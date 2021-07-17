using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    private bool firstAbilityReady, secondAbilityReady, thirdAbilityReady;
    

    private void RechargeAbilityFirst()
    {
        firstAbilityReady = true;
    }

    private void RechargeAbilitySecond()
    {
        secondAbilityReady = true;
    }

    private void RechargeAbilityThird()
    {
        thirdAbilityReady = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && firstAbilityReady)
        {
            firstAbilityReady = false;

        }
    }

    private void FirstListAbilities() //возможно переписать на enum или еще что-нибудь
    {

    }

    private void SecondListAbilities()
    {

    }

    private void ThirdListAbilities()
    {

    }
}
