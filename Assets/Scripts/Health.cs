using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    private float _currentHealth;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    public event Action<float> OnDamaged;
    public event Action<float> OnChangeHealth;
    public event Action OnDestroyed;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        float origin = _currentHealth;
        SetHealth(CurrentHealth - damage);

        if (origin != _currentHealth) {
            OnDamaged(_currentHealth - origin);
        }

        if (_currentHealth == 0)
        {
            DestroyBlock();
        }
    }

    public void DestroyBlock()
    {
        OnDestroyed();
    }

    private void SetHealth(float health)
    {
        float origin = _currentHealth;
        _currentHealth = Mathf.Clamp(health, 0, _maxHealth);
        if (origin != CurrentHealth)
        {
            OnChangeHealth(origin);
        }
    }
}
