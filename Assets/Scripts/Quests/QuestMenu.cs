using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
public class QuestMenu : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    public GameObject objShowDef;
    [SerializeField] private ScrollRect scrollRectActive, scrollRectComplete;
    [SerializeField] private float curYA, curYC, height, offset;
    [SerializeField] private GameObject questShowInfoPanel;
    [SerializeField] List<QuestButton> showQuestActive = new List<QuestButton>();
    [SerializeField] List<QuestButton> showQuestComplete = new List<QuestButton>();
    [SerializeField] private string folder, fileName;
    [SerializeField] private Text questDescription;
    public List<QuestData> _quests = new List<QuestData>();
    private QuestData quest;

    private void Awake()
    {
        TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);//путь к файлу
        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));//читаем текст
        int index = 0;
        while (reader.Read())//пока читается читаем 
        {
            quest = new QuestData();
            if(reader.IsStartElement("ID"))
                quest.id = reader.ReadElementString("ID");
            
            if(reader.IsStartElement("Name"))
                quest.name = reader.ReadElementString("Name");

            if (reader.IsStartElement("Description"))
                quest.description = reader.ReadElementString("Description");

            _quests.Add(quest);
            index++;

        }
        reader.Close();
        _quests.Remove(_quests[0]);//удаление последнего и первого элемента потому что они пустые
        _quests.Remove(_quests[_quests.Count - 1]);

    }
    void Start()
    {
        SetAllInActive();
        GetAllLinks();
    }

    private void GetAllLinks()
    {

    }

    public int GetQuestCount()
    {
        return _quests.Count;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Background.BackgroundActive == false)
            {
                questPanel.SetActive(true);
                Background.BackgroundActive = true;
            }
            else
            {
                questPanel.SetActive(false);
                Background.BackgroundActive = false;

                SetAllInActive();

            }
        }
    }

    private void SetAllInActive()
    {
        questPanel.SetActive(false);
        questShowInfoPanel.SetActive(false);
        questDescription.text = string.Empty;
    }

    public void AddActiveOrComplete(string IDQuest, bool isActive)//isActive тут нужна для проверки активен квест или нет
    {
        GameObject newObject = Instantiate(objShowDef, questPanel.transform);
        newObject.name = _quests[GetINT(IDQuest)].name;
        QuestButton newLoadObj = new QuestButton();
        newLoadObj.choiceQuest = newObject;
        newLoadObj.name = newObject.name;
        RectTransform rt = newObject.GetComponent<RectTransform>();

        rt.localPosition = Vector3.zero;
        rt.localScale = Vector3.one;

        height = rt.sizeDelta.y;

        Button tempButton = newObject.GetComponent<Button>();
        tempButton.GetComponent<Transform>().GetChild(0).GetComponent<Text>().text = newLoadObj.name;
        tempButton.onClick.AddListener(delegate { ShowInfoAboutQuest(newLoadObj.name); });

        if (isActive == true)
        {
            rt.SetParent(scrollRectActive.content);
            rt.anchoredPosition = new Vector2(0, -height / 2 - curYA);
            showQuestActive.Add(newLoadObj);
            curYA += height + offset;
            RectContent();
        }
        else
        {
            rt.SetParent(scrollRectComplete.content);
            rt.anchoredPosition = new Vector2(0, -height / 2 - curYC);
            showQuestComplete.Add(newLoadObj);
            DeleteQuest(GetNameById(IDQuest));
            curYC += height + offset;
            RectContent();
        }
    }

    public string GetNameById(string IDQuest)
    {
        return _quests[int.Parse(IDQuest)].name;
    }
    public void DeleteQuest(string name)
    {
        for (int i = 0; i < showQuestActive.Count; i++)
        {
            if(showQuestActive[i].name == name)
            {
                Destroy(showQuestActive[i].choiceQuest);
                showQuestActive.Remove(showQuestActive[i]);
                break;
            }
        }
    }


    private void ShowInfoAboutQuest(string name)
    {
        questShowInfoPanel.SetActive(true);
        questDescription.text = GetDescriptionList(name);
    }

    private string GetDescriptionList(string name)
    {
        for(int i = 0; i < _quests.Count; i++)
        {
            if(_quests[i].name == name)
            {
                return _quests[i].description;
            }
        }

        return string.Empty;
    }
    int GetINT(string text)//перевод из строки в числа
    {
        int value;
        if (int.TryParse(text, out value))
        {
            return value;
        }
        else
            return 0;
    }
    void RectContent()//косметические правки
    {
        scrollRectActive.content.sizeDelta = 
            new Vector2(scrollRectActive.content.sizeDelta.x * CoefficientScreenSize.GetCoefficient, curYA * CoefficientScreenSize.GetCoefficient);
        scrollRectComplete.content.sizeDelta = 
            new Vector2(scrollRectComplete.content.sizeDelta.x * CoefficientScreenSize.GetCoefficient, curYC * CoefficientScreenSize.GetCoefficient);
    }

}

[System.Serializable]

public class QuestButton
{
    public GameObject choiceQuest;
    public string name;
    public string description;
}
[System.Serializable]
public struct QuestData
{
    public string description;
    public string name, id;
}
