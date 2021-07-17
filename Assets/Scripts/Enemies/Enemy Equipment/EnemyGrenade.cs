using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenade : MonoBehaviour
{
    [SerializeField] private float _timeBeforeDestroy;
    [SerializeField] private float _mainDamage;
    [SerializeField] private float _explosionArea;
    private float _neededForce;
    private PlayerHealth player;
    internal float GetDamage
    {
        set
        {
            _mainDamage = value;//выставление урона исходя из скрипта стрельбы/турельки в этих скриптах (лучше убрать)
        }
        get
        {
            return _mainDamage;//сделать получение урона исходя из характеристик персонажа (добавить)
        }
    }
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        _neededForce = Vector3.Distance(transform.position, player.transform.position) * 50;
        rb.AddForce(transform.forward * _neededForce);
        Invoke(nameof(GrenadeExposion), _timeBeforeDestroy);
    }


    private void GrenadeExposion()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (_explosionArea > distance)
        {
            float damage = _mainDamage * (distance - (int)distance);
            player.Damage(damage);
        }
        Destroy(gameObject);
    }
}
