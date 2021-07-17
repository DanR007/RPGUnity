using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] internal int _maxCountAttacks;
    private int _countAttacks;
    [SerializeField] private float waitBeforeMinusAttack;
    public List<int> pointMeleeValue = new List<int>();
    public List<GameObject> pointsMelee = new List<GameObject>();
    private Coroutine reduceAttack;

    private void Start()
    {
        for (int i = 0; i < pointMeleeValue.Count; i++)
        {
            pointMeleeValue[i] = -1;
        }
    }

    internal int GetCountAttack
    {
        get
        {
            return _countAttacks;
        }
    }

    internal void AnAttackPlayer()
    {
        _countAttacks++;
        reduceAttack = StartCoroutine(ReducingCountAttack());
    }
    
    private IEnumerator ReducingCountAttack()
    {
        while(_countAttacks > 0)
        {
            yield return new WaitForSeconds(waitBeforeMinusAttack);
            _countAttacks--;
        }
    }
}
