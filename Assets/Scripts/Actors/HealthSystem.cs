using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private bool sfxVariance = false;
    private int maxHealth;

    private int health;

    [SerializeField]
    private bool isPlayer;

    [SerializeField]
    private bool isInvincible = false;

    [SerializeField]
    private AudioClip damageSFX;

    [SerializeField]
    private AudioClip altDamageSFX;

    [SerializeField]
    private float sfxVolume = 0.25f;

    public Action OnTakeDamage;
    public EventHandler OnDeath;
    public EventHandler<int> OnNewHealth;

    private void Awake()
    {
        health = maxHealth;
    }

    private void Start()
    {
        if (FeedbackManager.Instance.TryGetDictionaryValue("SFX", out int val))
        {
            if (val == 1)
            {
                sfxVariance = true;
                damageSFX = altDamageSFX;
            }
            else if (val == 2)
            {
                sfxVolume = 0f;
            }
        }
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

        AudioManager.PlaySFX(damageSFX, sfxVolume, 0, transform.position, sfxVariance);

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
