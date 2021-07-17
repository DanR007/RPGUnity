using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyRange : Enemy
{
    [SerializeField] private float shotDistance, warningDistance, timeBetweenRunAway;
    [SerializeField] private GameObject defProjectile;
    [SerializeField] private bool readyToRun = true;

    private void SetReadyToRun()
    {
        readyToRun = true;
    }
    private Vector3 GetFarPosition
    {
        get
        {
            Vector3 vect = new Vector3(Random.Range(warningDistance - (GetPlayer.transform.position.x - transform.position.x), warningDistance), 0,
                Random.Range(warningDistance - (GetPlayer.transform.position.z - transform.position.z), warningDistance));
            return transform.position + vect;
        }
    }
    private void Start()
    {
        SetDefaultSettings();
        StartCoroutine(CheckDistanceToPlayer());
    }
    private IEnumerator CheckDistanceToPlayer()
    {
        yield return null;
        LookAtPlayer();
        if (Vector3.Distance(transform.position, _target) <= 0.7f)
        {
            _notChangePosition = true;
        }

        if (GetDistance <= _triggerDistance && GetDistance > shotDistance && CheckPossibilityToFollowPlayer)
        {
            Move(GetPlayer.transform.position);
        }
        //враг работает на дальней дистанции
        if (GetDistance <= shotDistance)
        {
            if (GetDistance <= warningDistance && readyToRun && CheckPossibilityToFollowPlayer)
            {
                _notChangePosition = false;
                Move(GetFarPosition);
                readyToRun = false;
                Invoke(nameof(SetReadyToRun), timeBetweenRunAway);
            }
            else
            {
                LongFightDistance(defProjectile);//прописано в родительском скрипте
            }
        }

        
        StartCoroutine(CheckDistanceToPlayer());
    }
}
