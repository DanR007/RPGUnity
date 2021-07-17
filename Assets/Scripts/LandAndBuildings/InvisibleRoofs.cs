using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleRoofs : MonoBehaviour
{
    private MeshRenderer _mesh;

    private void Start()
    {
        _mesh = GetComponent<Transform>().GetChild(0).GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Movement>(out _))
        {
            _mesh.forceRenderingOff = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Movement>(out _))
        {
            _mesh.forceRenderingOff = false;
        }
    }
}
