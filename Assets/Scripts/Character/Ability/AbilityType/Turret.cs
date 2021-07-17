using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private List<Enemy> enemiesInRange = new List<Enemy>();
    private Enemy[] enemies;
    [SerializeField] private float _timeBetweenAttack, _damage, _workTime;
    [SerializeField] private float radiusAttack;
    [SerializeField] private GameObject projectile, gun;
    private Vector3 _rotationToTarget;
    [SerializeField] private Enemy targetEnemy;
    private bool isReadyToAttack;

    internal float SetWorkTime
    {
        set
        {
            _workTime = value;
        }
    }
    private void Start()
    {
        Invoke(nameof(DestroyTurret), _workTime);
        isReadyToAttack = true;
        StartCoroutine(SearchEnemy());
    }

    private IEnumerator SearchEnemy()
    {
        yield return null;

        GetEnemiesInRange();
        Check_Distance_And_Availability();
        
        if (targetEnemy == null)
        {
            GetEnemy();
        }
        else
        {
            _rotationToTarget = targetEnemy.transform.position - gun.transform.position;
            _rotationToTarget.y = 0;
            transform.rotation = Quaternion.LookRotation(_rotationToTarget);

            if (isReadyToAttack)
            {
                isReadyToAttack = false;
                Invoke(nameof(SetReadyToAttack), _timeBetweenAttack);
                Attack();
            }
            if (Check_Walls_Between_Turret_And_Enemy(targetEnemy.transform.position) == false)
            {
                targetEnemy = null;
            }
        }
        
        StartCoroutine(SearchEnemy());
    }

    private void GetEnemiesInRange()
    {
        enemies = FindObjectsOfType<Enemy>();
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemiesInRange.Contains(enemies[i]) == false &&
                Enemy_In_The_Affected_Area(enemies[i].transform.position))
            {
                enemiesInRange.Add(enemies[i]);
            }
        }
    }


    private void Check_Distance_And_Availability()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            if (enemiesInRange[i] == null || Enemy_In_The_Affected_Area(enemiesInRange[i].transform.position) == false)
            {

                if (enemiesInRange[i] == targetEnemy)
                {
                    targetEnemy = null;
                }
                enemiesInRange.Remove(enemiesInRange[i]);
            }
        }
    }

    private void SetReadyToAttack()
    {
        isReadyToAttack = true;
    }


    private bool Enemy_In_The_Affected_Area(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) <= radiusAttack;
    }


    private void GetEnemy()
    {
        Vector3 enemyPosition;
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            enemyPosition = enemiesInRange[i].transform.position;
            if(Check_Walls_Between_Turret_And_Enemy(enemyPosition))
            {
                targetEnemy = enemiesInRange[i];
            }
        }
    }

    private bool Check_Walls_Between_Turret_And_Enemy(Vector3 enemyPos)
    {
        Ray ray = new Ray(gun.transform.position, enemyPos - gun.transform.position);
        Debug.DrawRay(gun.transform.position, enemyPos - gun.transform.position, Color.red);
        return Physics.Raycast(ray, out RaycastHit hit, radiusAttack) && hit.collider.gameObject.TryGetComponent<Enemy>(out _);
    }
    private void Attack()
    {
        GameObject newProjectile = Instantiate(projectile);
        newProjectile.transform.position = gun.transform.position;
        newProjectile.transform.rotation = transform.rotation;
        newProjectile.GetComponent<Projectile>().GetDamage = _damage; //тут добавить увеличение урона по характеристикам
        
    }

    private void DestroyTurret()
    {
        Destroy(gameObject);
    }
}
