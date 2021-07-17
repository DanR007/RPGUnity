using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrenadeType
{
    electricalFrenade, frozeGrenade
}
public class Grenade : MonoBehaviour
{
    private int attackDamage;
    [SerializeField]
    private GrenadeType gType;
    [SerializeField] private float radius;
    //private Rigidbody rb;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float expTime;
    [SerializeField] private float reloadTime;
    private List<Enemy> enemiesInAffectedArea = new List<Enemy>();
    private Enemy[] allEnemy;
    public GrenadeType GType
    {
        get
        {
            return gType;//возвращаем вид снаряда
        }
    }
    public void SetGrenadeID(int id)
    {
        if (id == 1)
            Invoke(nameof(GrenadeExplosion), expTime);
        else
            Invoke(nameof(CryoGrenageExplosion), expTime);
    }
    internal float GetReloadTime
    {
        get
        {
            return reloadTime;
        }
    }
    private void GrenadeExplosion()
    {
        GetEnemyInArea();
        for (int i = 0; i < enemiesInAffectedArea.Count; i++)
        {
            float distPercent = Vector2.Distance(transform.position, enemiesInAffectedArea[i].transform.position) 
                - (int)Vector2.Distance(transform.position, enemiesInAffectedArea[i].transform.position);
            float dam = distPercent * attackDamage * 2;//сменить урон атаки на определенную цифру которая будет зависит от характеристик персонажа
            enemiesInAffectedArea[i].GetComponent<Enemy>().Damage(dam);
        }
        Destroy(gameObject);
    }
    private void CryoGrenageExplosion()
    {
        GetEnemyInArea();
        for (int i = 0; i < enemiesInAffectedArea.Count; i++)
        {
            if (enemiesInAffectedArea[i].TryGetComponent<Enemy>(out var da))
            {
                da.FreezingTime = 10f;//поменять значения на характеристики
            }
        }
        Destroy(gameObject);
    }
    private void GetEnemyInArea()//получение списка всех врагов в радиусе действия гранаты
    {
        allEnemy = FindObjectsOfType<Enemy>();
        if(allEnemy.Length > 0)
        {
            for(int i = 0; i < allEnemy.Length; i++)
            {
                if(Vector3.Distance(transform.position, allEnemy[i].transform.position) <= radius &&
                    enemiesInAffectedArea.Contains(allEnemy[i]) == false)
                {
                    enemiesInAffectedArea.Add(allEnemy[i]);
                }
            }
        }
    }
}
