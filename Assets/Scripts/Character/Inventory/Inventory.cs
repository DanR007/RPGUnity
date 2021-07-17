using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static int _intellect, _strength, _sleight, _charizma;

    private void Awake()
    {
        _intellect = 0;
        _charizma = 0;
        _sleight = 0;
        _strength = 0;
    }
    internal int Intellect
    {
        get
        {
            return _intellect;
        }
        set
        {
            
        }
    }
    internal int Strength
    {
        get
        {
            return _strength;
        }
        set
        {

        }
    }
    internal int Sleight
    {
        get
        {
            return _sleight;
        }
        set
        {

        }
    }
    internal int Charizma
    {
        get
        {
            return _charizma;
        }
        set
        {

        }
    }
}
