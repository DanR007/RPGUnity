using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoefficientScreenSize : MonoBehaviour
{
    internal static float GetCoefficient
    {
        get
        {
            return Screen.currentResolution.height / 1080f;
        }
    }
}
