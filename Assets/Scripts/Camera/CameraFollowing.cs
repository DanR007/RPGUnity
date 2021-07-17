using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    private Movement _player;
    [SerializeField] private float _maxDistanceToPlayer;
    private float startYPos;
    private Vector3 target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float _speed;
    private void Start()
    {
        _player = FindObjectOfType<Movement>();
        startYPos = transform.position.y - _player.transform.position.y;
    }
    private void Update()
    {
        if(RaycastHitDistantionToPlayer() > _maxDistanceToPlayer)
        {
            float nowYPos = transform.position.y - _player.transform.position.y;
            target = new Vector3(_player.transform.position.x + offset.x, transform.position.y + (startYPos - nowYPos), _player.transform.position.z + offset.z);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _speed);
        }


    }

    private float RaycastHitDistantionToPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.currentResolution.width/2, Screen.currentResolution.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return Vector3.Distance(hit.point, _player.transform.position);
        }
        else
            return 0;
    }
}
