using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public static int progress, tmp;
    public static int goal;
    private static Quest _internal;

    public QuestMenu questMenu;
    public static Quest Internal
    {
        get { return _internal; }
    }

    private void Start()
    {
        progress = 0;
        questMenu = FindObjectOfType<Movement>().GetComponent<QuestMenu>();
    }

    void Awake()
    {
        //ResetQuest();
        //GetAllLinks();
        progress = 0; // начальное значение статуса

        _internal = this;
    }

    public void GetAllLinks()
    {

    }

    public int questValue
    {
        get { return progress; }
    }

    public void QuestStatus(Status status, int id, int progress)
    {
        switch (status)
        {
            case Status.Active:
                SetActiveQuest(progress);
                if (questMenu == null)
                {
                    Start();
                }
                questMenu.AddActiveOrComplete(id.ToString(), true);
                break;
            case Status.Complete:
                if (questMenu == null)
                {
                    Start();
                }
                questMenu.AddActiveOrComplete(id.ToString(), false);

                SetCompleteQuest();
                break;
            case Status.Disable:
                if (questMenu == null)
                {
                    Start();
                }
                questMenu.DeleteQuest(questMenu.GetNameById(id.ToString()));
                ResetQuest();
                break;
        }
    }

    public virtual void SetActiveQuest(int progressSet)
    {
        //enabled = true;
        progress = progressSet;
        Debug.Log("ha");
    }
    public virtual void SetCompleteQuest()
    {
        //enabled = false;
        progress = -1; // квест сдан
    }
    public virtual void ResetQuest()
    {
        // enabled = false;
        Debug.Log(progress);
        progress = 0;
    }
}
