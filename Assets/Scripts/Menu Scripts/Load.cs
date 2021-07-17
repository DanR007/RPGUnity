using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    [SerializeField] private GameObject loadButtonDefault;
    [SerializeField] private GameObject loadViewPanel;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject pauseMenu;
    FileInfo[] savingFiles;
    internal int _index;
    private const string _nameFolder = "Saves";
    [SerializeField] private List<LoadObject> loadButtons = new List<LoadObject>();
    [SerializeField] private Button returnToPauseMenu;
    [SerializeField] internal float _height, _offset, _curY;

    private PlayerHealth _playerHealth;
    private Movement _playerPosition;
    private Inventory _playerInventory;

    private void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _playerPosition = _playerHealth.GetComponent<Movement>();
        _playerInventory = _playerHealth.GetComponent<Inventory>();
        returnToPauseMenu.onClick.AddListener(() => ReturnToMenu());
        if (NameFileSave.FullFileName != string.Empty)
        {
            LoadSavingSceneData(NameFileSave.FullFileName);
        }
    }

    internal void UpdateSavingData()
    {
        foreach (LoadObject loadButton in loadButtons)
        {
            Destroy(loadButton.showObject);
        }
        loadButtons.Clear();
        string[] file_list = Directory.GetFiles(Application.dataPath + "/" + _nameFolder, "*.xml");
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/" + _nameFolder);
        savingFiles = dirInfo.GetFiles("*.xml");
        Array.Sort(savingFiles,
                new Comparison<FileInfo>(
                    (file1, file2) => file2.CreationTime.CompareTo(file1.CreationTime))
                    );


        if (savingFiles.Length >= 80)
        {
            DeleteSaveData();
        }

        foreach (FileInfo file in savingFiles)
        {
            AddLoadData(Path.GetFileNameWithoutExtension(file.Name));
        }
    }

    private void DeleteSaveData()
    {
        for (int i = 80; i < savingFiles.Length; i++)
            File.Delete(savingFiles[i].FullName);
    }
    internal void AddLoadData(string fileName)
    {
        GameObject newObject = Instantiate(loadButtonDefault, loadViewPanel.transform);
        newObject.name = fileName;

        LoadObject newLoadObject = new LoadObject();
        newLoadObject.showObject = newObject;
        newLoadObject._name = fileName;

        RectTransform rt = newObject.GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero;
        rt.localScale = Vector3.one;
        rt.SetParent(scrollRect.content);
        rt.anchoredPosition = new Vector2(0, -_height / 2 - _curY);

        Button newButton = newObject.GetComponent<Button>();
        newButton.transform.GetChild(0).GetComponent<Text>().text
            = GetNameSavingScene(fileName);
        newButton.transform.GetChild(1).GetComponent<Text>().text
            = File.GetCreationTime(Application.dataPath + "/" + _nameFolder + "/" + fileName + ".xml").ToString();
        newButton.onClick.AddListener(() => LoadSavingScene(fileName));

        _height = rt.sizeDelta.y;
        _curY += _height + _offset;
        scrollRect.content.sizeDelta =
            new Vector2(scrollRect.content.sizeDelta.x * CoefficientScreenSize.GetCoefficient, _curY * CoefficientScreenSize.GetCoefficient);

        loadButtons.Add(newLoadObject);
    }
    private void LoadSavingSceneData(string fullFileName)
    {
        XmlTextReader reader = new XmlTextReader(fullFileName);

        while(reader.Read())
        {
            if (reader.IsStartElement("Scene"))
            {
                //do nothing
            }
            if (reader.IsStartElement("PlayerInfo"))
            {
                _playerHealth.SetSavingHealth = GetFloat(reader.GetAttribute("Health"));

                _playerPosition.transform.position = 
                    new Vector3(GetFloat(reader.GetAttribute("x")), GetFloat(reader.GetAttribute("y")), GetFloat(reader.GetAttribute("z")));

                _playerInventory.Strength = int.Parse(reader.GetAttribute("Strength"));
                _playerInventory.Intellect = int.Parse(reader.GetAttribute("Intellect"));
                _playerInventory.Sleight = int.Parse(reader.GetAttribute("Sleight"));
                _playerInventory.Charizma = int.Parse(reader.GetAttribute("Charizma"));

            }
            if (reader.IsStartElement("Quests"))
            {
                int i = 0;
                while (reader.ReadToFollowing("Quest"))
                {
                    QuestManager.LoadStatus(i.ToString(), (int)GetFloat(reader.GetAttribute("id" + i.ToString())));
                    i++;
                }

            }
        }

        reader.Close();
        NameFileSave.FullFileName = string.Empty;
    }


    public void LoadSavingScene(string fileName)
    {
        Time.timeScale = 1;
        loadViewPanel.SetActive(false);
        XmlTextReader reader;
        reader = new XmlTextReader(Application.dataPath + "/" + _nameFolder + "/" + fileName + ".xml");

        while (reader.Read())
        {
            if (reader.IsStartElement("Scene"))
            {
                NameFileSave.FullFileName = Application.dataPath + "/" + _nameFolder + "/" + fileName + ".xml";
                SceneManager.LoadScene(int.Parse(reader.GetAttribute("id")));
            }
        }
        reader.Close();
    }


    private string GetNameSavingScene(string fileName)
    {
        XmlTextReader reader;
        reader = new XmlTextReader(Application.dataPath + "/" + _nameFolder + "/" + fileName + ".xml");
        string name = string.Empty;
        while (reader.Read())
        {
            if (reader.IsStartElement("Scene"))
            {
                name = reader.GetAttribute("Name");
            }
        }
        reader.Close();
        return name;
    }

    private void ReturnToMenu()
    {
        pauseMenu.SetActive(true);
        loadPanel.SetActive(false);
    }
    private float GetFloat(string text)
    {
        float value;
        if(float.TryParse(text, out value))
        {
            return value;
        }
        else
        {
            return 0;
        }
    }
}


public class LoadObject
{
    public string _name;
    public GameObject showObject;
}
