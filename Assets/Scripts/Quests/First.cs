using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Movement>(out _))
        {
            FirstQuest.UpdateTarget();
            Destroy(gameObject);
        }
    }


}
