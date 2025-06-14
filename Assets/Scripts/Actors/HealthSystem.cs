using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float maxHealth;

    private float health;

    [SerializeField]
    private bool isPlayer;

    [SerializeField]
    private bool isInvincible = false;

    [SerializeField]
    private AudioClip damageSFX;

    public Action OnTakeDamage;
    public EventHandler OnDeath;
    public EventHandler<float> OnNewHealth;

    private void Awake()
    {
        health = maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }

        health = Mathf.Max(0, health - damage);

        OnNewHealth?.Invoke(this, health / maxHealth);

        Debug.Log(gameObject.name + "damage taken");

        if (health == 0f)
        {
            Die();
        }
        else
        {
            OnTakeDamage?.Invoke();
        }
    }

    // public void Heal(float amountToHeal)
    // {
    //     health = Mathf.Min(maxHealth, health + amountToHeal);
    //     OnNewHealth?.Invoke(this, health / maxHealth);
    // }

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
        Debug.Log(gameObject.name + "is Dead");
        //Destroy(gameObject);
    }
}
