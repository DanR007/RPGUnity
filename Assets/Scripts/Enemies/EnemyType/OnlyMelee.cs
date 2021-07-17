using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyMelee : Enemy
{
    [SerializeField] private float _meleeFightDistance, _meleeGetPointDistance;
    [SerializeField] private float rangeMeleeAttack;
    [SerializeField] private float _meleeDamage;

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

        if (GetDistance <= _triggerDistance && _notFrozen && _notChangePosition && _notStanLock)
        {
            Move(GetPlayer.transform.position);
        }
        if (GetDistance > _meleeGetPointDistance + 2f)
        {
            if (index != -1)
                GetEnemyManager.pointMeleeValue[index] = -1;


            index = -1;
            pointNearPlayer = Vector3.zero;
        }
        else
        {
            if (_meleeGetPointDistance > GetDistance)
            {
                CloseDistance(_meleeDamage, _meleeFightDistance);
            }
        }
        StartCoroutine(CheckDistanceToPlayer());
    }

    internal override void CloseDistance(float damage, float meleeFightDistance)
    {
        if (_notFrozen && _notChangePosition && _notStanLock)
        {

            if (index == -1)
            {
                for (int i = 0; i < GetEnemyManager.pointMeleeValue.Count; i++)
                {
                    if (GetEnemyManager.pointMeleeValue[i] == -1 && pointNearPlayer == Vector3.zero)
                    {
                        pointNearPlayer = GetEnemyManager.pointsMelee[i].transform.position;

                        GetEnemyManager.pointMeleeValue[i] = i;

                        index = i;
                    }
                }
            }

            if (pointNearPlayer != Vector3.zero)
                Move(pointNearPlayer);

            if (_meleeFightDistance <= GetDistance)
            {
                Stop();
                if (_notCoolDown && pointNearPlayer != Vector3.zero)
                {
                    AttackMelee(damage);
                }
            }

        }
        else
        {
            Stop();
        }
    }
}
