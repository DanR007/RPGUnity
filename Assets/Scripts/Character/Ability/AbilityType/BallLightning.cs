using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLightning : MonoBehaviour
{

    internal float SetTimeBeforeDestroy
    {
        set
        {
            Invoke(nameof(DestroyLighting), value);
        }
    }
        
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.Damage(10);//поменять значение на характеристику перса
        }
    }

    private void DestroyLighting()
    {
        Destroy(gameObject);
    }
}
