using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    private PlayerHealth _player;
    private QuestMenu questMenu;
    private Inventory inventory;
    private Load load;
    [SerializeField] private GameObject pauseMenu;
    private void Start()
    {
        _player = FindObjectOfType<PlayerHealth>();
        inventory = _player.GetComponent<Inventory>();
        questMenu = _player.GetComponent<QuestMenu>();
        load = Camera.main.GetComponent<Load>();
        if (NameFileSave.FullFileName == string.Empty)
        {
            Invoke(nameof(CreateSaveData), 2f);//создание сохранения при переходе на другую сцену
        }
    }
    internal void CreateSaveData()
    {
        Directory.CreateDirectory(Application.dataPath + "/Saves");
        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        string path = Application.dataPath + "/Saves/" + fileName + ".xml";
        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration xmldecl;
        xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
        xmldecl.Encoding = "utf-8";
        XmlNode userNode, userNode2;
        XmlAttribute userInventory;
        XmlAttribute posX = xmlDoc.CreateAttribute("x");
        XmlAttribute posY = xmlDoc.CreateAttribute("y");
        XmlAttribute posZ = xmlDoc.CreateAttribute("z");
        XmlNode rootNode = xmlDoc.CreateElement("Save");

        xmlDoc.AppendChild(rootNode);
        xmlDoc.InsertBefore(xmldecl, rootNode);

        userNode = xmlDoc.CreateElement("Scene");
        userInventory = xmlDoc.CreateAttribute("id");
        userInventory.Value = SceneManager.GetActiveScene().buildIndex.ToString();
        userNode.Attributes.Append(userInventory);

        userInventory = xmlDoc.CreateAttribute("Name");
        userInventory.Value = SceneManager.GetActiveScene().name;
        userNode.Attributes.Append(userInventory);

        rootNode.AppendChild(userNode);


        XmlNode playerInfo = xmlDoc.CreateElement("PlayerInfo");
        posX.Value = _player.transform.position.x.ToString();
        playerInfo.Attributes.Append(posX);
        posY.Value = _player.transform.position.y.ToString();
        playerInfo.Attributes.Append(posY);
        posZ.Value = _player.transform.position.z.ToString();
        playerInfo.Attributes.Append(posZ);


        XmlAttribute health = xmlDoc.CreateAttribute("Health");
        health.Value = _player.GetHealth.ToString();
        playerInfo.Attributes.Append(health);

        XmlAttribute characteristic = xmlDoc.CreateAttribute("Intellect");
        characteristic.Value = inventory.Intellect.ToString();
        playerInfo.Attributes.Append(characteristic);

        characteristic = xmlDoc.CreateAttribute("Strength");
        characteristic.Value = inventory.Strength.ToString();
        playerInfo.Attributes.Append(characteristic);

        characteristic = xmlDoc.CreateAttribute("Charizma");
        characteristic.Value = inventory.Charizma.ToString();
        playerInfo.Attributes.Append(characteristic);

        characteristic = xmlDoc.CreateAttribute("Sleight");
        characteristic.Value = inventory.Sleight.ToString();
        playerInfo.Attributes.Append(characteristic);

        rootNode.AppendChild(playerInfo);


        //эта штука нужна если кое-кому все-таки придёт в голову идея оставить сундук
        //ну или квестовые предметы в инвентаре (их придется заново писать)

        //userNode = xmlDoc.CreateElement("Chest");
        //for (int j = 0; j < che.chestItems.Count; j++)
        //{
        //    userInventory = xmlDoc.CreateAttribute("id" + j.ToString());
        //    userInventory.Value = che.chestItems[j].id.ToString();
        //    userNode.Attributes.Append(userInventory);
        //}
        //rootNode.AppendChild(userNode);



        userNode = xmlDoc.CreateElement("Quests");
        for (int j = 0; j < questMenu.GetQuestCount(); j++)
        {
            userNode2 = xmlDoc.CreateElement("Quest");
            userInventory = xmlDoc.CreateAttribute("id" + j.ToString());
            userInventory.Value = QuestManager.GetCurrentValue(j.ToString()).ToString();
            userNode2.Attributes.Append(userInventory);
            userNode.AppendChild(userNode2);
        }
        rootNode.AppendChild(userNode);


        xmlDoc.Save(path);

        load.AddLoadData(Path.GetFileNameWithoutExtension(path));
        pauseMenu.SetActive(false);
    }
}
