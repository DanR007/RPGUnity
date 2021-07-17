﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float _timeBeforeDestroy;
    [SerializeField] private float _damage;

    internal float GetDamage
    {
        set
        {
            _damage = value;//выставление урона исходя из скрипта стрельбы/турельки в этих скриптах (лучше убрать)
        }
        get
        {
            return _damage;//сделать получение урона исходя из характеристик персонажа (добавить)
        }
    }
    void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
        Invoke(nameof(DestroyProjectile), _timeBeforeDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out var player))
        {
            player.Damage(GetDamage);
            Destroy(gameObject);
        }
        else
        {
            if (other.gameObject.TryGetComponent<WallsAndLand>(out _))
            {
                Destroy(gameObject);
            }
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
