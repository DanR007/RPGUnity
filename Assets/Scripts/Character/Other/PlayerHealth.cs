using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private const float _maxHealth = 1000;
    private static float _health;
    private bool isInvulnerability;
    public Text healthText;

    internal float SetInvulnerabilityDuration//продолжительность неуязвимости
    {
        set
        {
            isInvulnerability = true;
            Invoke(nameof(SetVulnerability), value);
        }
    }

    private void SetVulnerability()
    {
        isInvulnerability = false;
    }
    private void Start()
    {
        _health = _maxHealth;
        healthText.text = _health.ToString();
        isInvulnerability = false;
    }
    
    internal float GetHealth
    {
        get
        {
            return _health;
        }
    }

    internal float SetSavingHealth
    {
        set
        {
            _health = value;
        }
    }
    internal void Damage(float damage)
    {
        if(isInvulnerability == false)//проверка на отсутствие неуязвимостей
        {
            if(_health - damage > 0)
            {
                isInvulnerability = true; //чтобы игрока сразу не съели
                _health -= damage;
                healthText.text = _health.ToString();
                Invoke(nameof(SetVulnerability), 0.1f);
            }
            else
            {
                //GameOver
            }
        }
    }
}
