using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCat : MonoBehaviour
{
    [SerializeField] private float _speed, _timeBetweenGrapping, _defTimeCooldown, _grapDistantion;
    private Vector3 target;
    private float distantionToTarget;


    //вызывается из скрипта способностей для получения времени перезарядки
    internal float GetTimeBetweenGrapping
    {
        get
        {
            return _timeBetweenGrapping;
        }
    }


    private void Start()
    {
        target = Vector3.zero;
    }
    //выбираем место куда будем притягиваться
    internal void StartGrapping()
    {
        RaycastHit mousePoint;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out mousePoint))
        {
            if (mousePoint.collider.gameObject.TryGetComponent<Enemy>(out var enemy)
                || mousePoint.collider.gameObject.TryGetComponent<WallsAndLand>(out _))
            {

                target = mousePoint.point;

                Vector3 direction = target;
                direction -= transform.position;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);


                //добавить время кулдауна цели

                MoveToTarget();
            }
        }
    }


    //двигаемся к цели в которую ткнули 
    private void MoveToTarget()
    {
        distantionToTarget = Vector3.Distance(target, transform.position);
        if(distantionToTarget >= 1.2 && target != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _speed);
        }
        else
        {
            target = Vector3.zero;
        }
    }
}
