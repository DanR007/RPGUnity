using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AllQuests")]
public class AllQuests : ScriptableObject
{
    public static FirstQuest f;
    public MonoBehaviour[] nameGameObjectQuest;
}
