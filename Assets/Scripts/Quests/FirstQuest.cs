using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstQuest : Quest
{
    [SerializeField] private static First[] target;

    private void Start()
    {
        if (QuestManager.nameQuest.Contains(this) == false)
        {
            QuestManager.nameQuest.Add(this);
        }
        Debug.Log(QuestManager.nameQuest.Count);
        progress = 0;
        target = FindObjectsOfType<First>();
        goal = target.Length;
        ResetQuest();
        questMenu = FindObjectOfType<Movement>().GetComponent<QuestMenu>();

    }

    internal static void UpdateTarget()
    {
        tmp++;
        if(tmp >= goal)
        {
            progress = 2;
        }
    }
    //public static int questValue
    //{
    //    get { return progress; }
    //}
    public override void SetActiveQuest(int progressSet)
    {
        progress = progressSet;
        //enabled = true;
        if (progressSet == 1)
        {

            foreach (First obj in target)
            {
                obj.gameObject.SetActive(true);
            }
        }
    }
    public override void SetCompleteQuest()
    {
        //enabled = false;
        progress = -1; // квест сдан
    }
    public override void ResetQuest()
    {
        //enabled = false;
        progress = 0;
        foreach (First obj in target)
        {
            obj.gameObject.SetActive(false);
        }
    }
}
