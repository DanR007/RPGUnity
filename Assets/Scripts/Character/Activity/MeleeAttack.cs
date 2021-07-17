using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private GameObject pointOfDamage;
    [SerializeField] private float _meleeDamage, _damageRange, _timeBetweenAttack, _stanLock;
    private bool isReadyToAttack;
    private List<Enemy> enemiesInRange = new List<Enemy>();
    private Enemy[] enemies;



    private void SetReadyToAttack()
    {
        isReadyToAttack = true;
    }
    private void Start()
    {
        isReadyToAttack = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && isReadyToAttack)
        {
            isReadyToAttack = false;
            Invoke(nameof(SetReadyToAttack), _timeBetweenAttack);
            Attack();
        }
    }
    
    private void Attack()
    {
        GetEnemyInRange();
        for(int i = 0; i < enemiesInRange.Count; i++)
        {
            enemiesInRange[i].Damage(_meleeDamage);//проставить урон по уровню и хар-кам перса
            enemiesInRange[i].StanLockDuration = _stanLock;
        }
        enemiesInRange.Clear();
    }

    private void GetEnemyInRange()
    {
        enemies = FindObjectsOfType<Enemy>();
        if (enemies != null)
            for (int i = 0; i < enemies.Length; i++)
            {
                if (Vector3.Distance(enemies[i].transform.position, pointOfDamage.transform.position) <= _damageRange
                    && enemiesInRange.Contains(enemies[i]) == false)
                {
                    enemiesInRange.Add(enemies[i]);
                }
            }
    }
}
