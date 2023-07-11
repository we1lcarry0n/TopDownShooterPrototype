using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private Image healthBar;
    private float currentHealth;

    public event EventHandler Death;

    private void Awake()
    {
        if (this.gameObject.CompareTag("Player"))
        {
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        }
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (healthBar != null)
        {
            healthBar.fillAmount = (currentHealth / maxHealth);
        }
        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        Death?.Invoke(this, EventArgs.Empty);
    }
}
