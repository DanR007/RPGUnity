using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private static bool _background;
    private void Start()
    {
        _background = false;
    }
    internal static bool BackgroundActive
    {
        set
        {
            _background = value;
        }
        get
        {
            return _background;
        }
    }
}
