using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Universal : Enemy
{
    [SerializeField] private float shotDistance, meleeFightDistance;
    [SerializeField] private GameObject defProjectile;
    [SerializeField] private float rangeMeleeAttack;
    [SerializeField] private float meleeDamage;

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
        if (GetDistance <= shotDistance && GetDistance > meleeFightDistance)
        {
            pointNearPlayer = Vector3.zero;
            LongFightDistance(defProjectile);//прописано в родительском скрипте
        }

        //враг работает на близкой дистанции
        if (GetDistance < shotDistance - 4f)
        {
            CloseDistance(meleeDamage, meleeFightDistance);
        }
        StartCoroutine(CheckDistanceToPlayer());
    }


}
