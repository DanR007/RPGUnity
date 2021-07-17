using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private ButtonComponent button;
	private const string folder = "Russian";
	[SerializeField] private int offset = 20;
	[SerializeField] private GameObject scrollView;
	private string fileName, lastName;
	private List<Dialogue> node;
	private Dialogue dialogue;
	private Answer answer;
	private float curY, height;
	private List<RectTransform> buttons = new List<RectTransform>();
	private static DialogueManager _internal;
	private static bool _active;
	private Movement player;
	public void DialogueStart(string name)
	{
		if (name == string.Empty)
		{
			return;
		}
		Background.BackgroundActive = true;
		fileName = name;
		Load();
	}

	private float SetCoefficient()
	{
		int n = Screen.currentResolution.width;
		return (float)n / 1920f;
	}
	public static DialogueManager Internal
	{
		get { return _internal; }
	}

	public static bool isActive
	{
		get { return _active; }
	}

	void Awake()
	{
		_internal = this;
		CloseWindow();
	}

	private void Start()
	{
		player = FindObjectOfType<Movement>();
		RectTransform rt = scrollView.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x * SetCoefficient(), rt.sizeDelta.y * SetCoefficient());
	}
	void Load()
	{
		if (lastName == fileName)
		{
			BuildDialogue(0);
			return;
		}

		node = new List<Dialogue>();
		try
		{
			TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);
			XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));

			int index = 0;
			while (reader.Read())
			{

				if (reader.IsStartElement("node"))
				{
					dialogue = new Dialogue();
					dialogue.answer = new List<Answer>();
					dialogue.npcText = reader.GetAttribute("npcText");
					dialogue.id = GetINT(reader.GetAttribute("id"));
					dialogue.reward = reader.GetAttribute("reward");
					dialogue.rewardCount = GetINT(reader.GetAttribute("rewardCount"));
					dialogue.rewardID = GetINT(reader.GetAttribute("rewardID"));
					node.Add(dialogue);

					XmlReader inner = reader.ReadSubtree();
					while (inner.ReadToFollowing("answer"))
					{
						answer = new Answer();
						answer.text = reader.GetAttribute("text");
						answer.toNode = GetINT(reader.GetAttribute("toNode"));
						answer.exit = GetBOOL(reader.GetAttribute("exit"));
						answer._isImpactQuest = GetBOOL(reader.GetAttribute("impactOnQuest"));
						answer.questStatus = GetINT(reader.GetAttribute("questStatus"));
						answer.questValue = GetINT(reader.GetAttribute("questValue"));
						answer.questValueGreater = GetINT(reader.GetAttribute("questValueGreater"));
						answer.questID = reader.GetAttribute("questID");
						answer.needIntellect = GetINT(reader.GetAttribute("NeedIntellect"));
						answer.needCharizma = GetINT(reader.GetAttribute("NeedCharizma"));
						
						node[index].answer.Add(answer);
					}
					inner.Close();

					index++;
				}
			}
			lastName = fileName;
			reader.Close();
		}
		catch (System.Exception error)
		{
			Debug.Log(this + " ошибка чтения файла диалога: " + fileName + ".xml | Error: " + error.Message);
			CloseWindow();
			lastName = string.Empty;
		}

		BuildDialogue(0);
	}
	void AddToList(bool exit, int toNode, string text, int questStatus, string questID, bool isActive, bool isImpactQuest)
	{
		ButtonComponent clone = Instantiate(button) as ButtonComponent;
		clone.gameObject.SetActive(true);
		clone.rect.SetParent(scrollRect.content);
		clone.rect.localScale = Vector3.one;
		if (isActive)
		{
			clone.text.text = text;
			clone.rect.sizeDelta = new Vector2(clone.rect.sizeDelta.x, clone.text.preferredHeight + offset);
		}
		else
		{
			StartCoroutine(NPCText(text, clone));
			clone.rect.sizeDelta = new Vector2(clone.rect.sizeDelta.x, (clone.text.preferredHeight + offset) * 2);
		}
		clone.button.interactable = isActive;
		height = clone.rect.sizeDelta.y;
		clone.rect.anchoredPosition = new Vector2(0, -height / 2 - curY);



		buttons.Add(clone.rect);
		if (exit)
		{
			SetExitDialogue(clone.button);
			if (isImpactQuest) SetQuestStatus(clone.button, questStatus, questID);
		}
		else
		{
			SetNextDialogue(clone.button, toNode);
			if (isImpactQuest) SetQuestStatus(clone.button, questStatus, questID);
		}
		curY += height + offset;
		RectContent();
	}
	IEnumerator NPCText(string text, ButtonComponent clone)
	{
		foreach (char letter in text.ToCharArray())
		{
			if (clone != null)
			{
				clone.text.text += letter;
				yield return new WaitForSeconds(0.02f);
			}
			else
			{
				break;
			}
		}
	}
	private void BackgroundDisabled()
	{
		Background.BackgroundActive = false;
		CloseWindow();
	}
	void RectContent()
	{
		scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x * SetCoefficient(), curY * SetCoefficient());
		scrollRect.content.anchoredPosition = Vector2.zero;
	}

	void ClearDialogue()
	{
		curY = offset;
		foreach (RectTransform b in buttons)
		{
			Destroy(b.gameObject);
		}
		buttons = new List<RectTransform>();
		RectContent();
	}

	void SetQuestStatus(Button button, int progressSet, string id)
	{
		string t = id + "|" + progressSet;
		button.onClick.AddListener(() => QuestStatus(t));
	}

	void SetNextDialogue(Button button, int id)
	{

		button.onClick.AddListener(() => BuildDialogue(id));
	}

	void SetExitDialogue(Button button)
	{
		button.onClick.AddListener(() => BackgroundDisabled());
	}

	void QuestStatus(string s)
	{
		string[] t = s.Split(new char[] { '|' });

		if (t[1] != "-1" && t[1] != "0")
		{
			QuestManager.SetQuestStatus(t[0], Status.Active, int.Parse(t[1]));
		}
		else if (t[1] == "0")
		{
			QuestManager.SetQuestStatus(t[0], Status.Disable, int.Parse(t[1]));
		}
		else if (t[1] == "-1")
		{
			QuestManager.SetQuestStatus(t[0], Status.Complete, int.Parse(t[1]));
			if (player.TryGetComponent<Inventory>(out var inventory))
			{
				//награда
			}
		}
	}

	void CloseWindow()
	{
		_active = false;
		scrollRect.gameObject.SetActive(false);
	}

	void ShowWindow()
	{
		scrollRect.gameObject.SetActive(true);
		_active = true;
	}

	int FindNodeByID(int i)
	{
		int j = 0;
		foreach (Dialogue d in node)
		{
			if (d.id == i) return j;
			j++;
		}

		return -1;
	}
	void BuildDialogue(int current)
	{
		ClearDialogue();

		int j = FindNodeByID(current);

		AddToList(false, 0, node[j].npcText, 0, string.Empty, false, false);

		for (int i = 0; i < node[j].answer.Count; i++)
		{

			int value = QuestManager.GetCurrentValue(node[j].answer[i].questID);

			if (value >= node[j].answer[i].questValueGreater && node[j].answer[i].questValueGreater != 0 ||
				node[j].answer[i].questValue == value && node[j].answer[i].questValueGreater == 0 ||
				node[j].answer[i].questID == null)
			{
				if (node[j].answer[i].needIntellect <= player.GetComponent<Inventory>().Intellect
				&& node[j].answer[i].needCharizma <= player.GetComponent<Inventory>().Charizma)
				{
					AddToList(node[j].answer[i].exit, node[j].answer[i].toNode, node[j].answer[i].text, node[j].answer[i].questStatus, node[j].answer[i].questID, true, node[j].answer[i]._isImpactQuest);
				}
			}
		}

		EventSystem.current.SetSelectedGameObject(scrollRect.gameObject);
		ShowWindow();
	}

	int GetINT(string text)
	{
		int value;
		if (int.TryParse(text, out value))
		{
			return value;
		}
		else
			return 0;
	}

	bool GetBOOL(string text)
	{
		bool value;
		if (bool.TryParse(text, out value))
		{
			return value;
		}
		return false;
	}
}

class Dialogue
{
	public int id, rewardID, rewardCount;
	public string npcText, reward;
	public List<Answer> answer;
}


class Answer
{
	public string text, questID;
	public int toNode, questValue, questValueGreater, questStatus, needIntellect, needCharizma;
	public bool exit, _isImpactQuest;
}