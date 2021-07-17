using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameFileSave : MonoBehaviour
{
    private static string _nameFile = string.Empty;

    private void Start()
    {
        _nameFile = string.Empty;
    }
    internal static string FullFileName
    {
        get
        {
            return _nameFile;
        }
        set
        {
            _nameFile = value;
        }
    }
}
