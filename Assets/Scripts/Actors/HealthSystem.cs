using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private int maxHealth;

    private int health;

    [SerializeField]
    private bool isPlayer;

    [SerializeField]
    private bool isInvincible = false;

    [SerializeField]
    private AudioClip damageSFX;

    public Action OnTakeDamage;
    public EventHandler OnDeath;
    public EventHandler<int> OnNewHealth;

    private void Awake()
    {
        health = maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        health = Mathf.Max(0, health - damage);

        OnNewHealth?.Invoke(this, health);

        //Debug.Log(gameObject.name + "damage taken");

        if (health == 0f)
        {
            Die();
        }
        else
        {
            OnTakeDamage?.Invoke();
        }
    }

    public void Heal(int amountToHeal)
    {
        health = Mathf.Min(maxHealth, health + amountToHeal);
        OnNewHealth?.Invoke(this, health);
    }

    public float GetHealth()
    {
        return health;
    }

    public bool GetIsPlayer()
    {
        return isPlayer;
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
        //Debug.Log(gameObject.name + "is Dead");
        //Destroy(gameObject);
    }
}
