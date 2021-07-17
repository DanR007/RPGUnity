using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
public enum Status { Disable, Active, Complete };
public class QuestManager : MonoBehaviour
{
    //public static AllQuests allQuests = Resources.Load<AllQuests>("");
    public static List<Quest> nameQuest = new List<Quest>()
    {
        //allQuests.nameGameObjectQuest[0].GetComponent<FirstQuest>()
        //allQuests.nameGameObjectQuest[1].GetComponent<SecondQuest>()
    };
    
    
    public static int GetCurrentValue(string questID)
    {
        if (questID != null)
        {
            
            return nameQuest[int.Parse(questID)].questValue;
        }
        else
            return -2;//-1 используется для сданного квеста
    }

    public static void LoadStatus(string questID, int progress)
    {
        if (progress == -1)
        {
            SetQuestStatus(questID, Status.Complete, progress);
        }
        else
        {
            if (progress == 0)
            {
                SetQuestStatus(questID, Status.Disable, progress);

            }
            else
            {
                SetQuestStatus(questID, Status.Active, progress);

            }
        }
    }
    public static void SetQuestStatus(string questID, Status status, int progress)
    {
        nameQuest[int.Parse(questID)].QuestStatus(status, int.Parse(questID), progress);
    }
}