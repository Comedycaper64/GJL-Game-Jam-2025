using System;
using UnityEngine;

public class RangedEnemyStateMachine : StateMachine
{
    private bool moving = false;
    private Vector2 moveDirection;
    private bool sfxVariance;
    private Transform playerTransform;
    private HealthSystem health;
    private RangedEnemyStats stats;

    [SerializeField]
    private Collider2D bodyCollider;

    [SerializeField]
    private Rigidbody2D enemyRB;

    [SerializeField]
    private GameObject enemyVisual;

    [SerializeField]
    private AudioClip shootSFX;

    [SerializeField]
    private AudioClip altShootSFX;

    [SerializeField]
    private float sfxVolume = 0.25f;

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        stats = GetComponent<RangedEnemyStats>();

        ToggleCollider(false);
        health.SetMaxHealth(stats.health);
        health.OnTakeDamage += HealthSystem_OnTakeDamage;
        health.OnDeath += HealthSystem_OnDeath;
    }

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;

        ResetEnemy();

        if (FeedbackManager.Instance.TryGetDictionaryValue("SFX", out int val))
        {
            if (val == 1)
            {
                sfxVariance = true;
                shootSFX = altShootSFX;
            }
            else if (val == 2)
            {
                sfxVolume = 0f;
            }
        }
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= HealthSystem_OnTakeDamage;
        health.OnDeath -= HealthSystem_OnDeath;
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            enemyRB.MovePosition(
                enemyRB.position + (moveDirection * stats.movementSpeed * Time.fixedDeltaTime)
            );
        }
    }

    public override void SpawnEnemy()
    {
        health.SetMaxHealth(stats.health);
        SwitchState(new RangedEnemySpawnState(this));
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    private void HealthSystem_OnTakeDamage()
    {
        //Damage feedback
        //Play hit sound?
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        SwitchState(new RangedEnemyDeadState(this));
        OnEnemyDeath?.Invoke();
    }

    public RangedEnemyStats GetStats()
    {
        return stats;
    }

    public HealthSystem GetHealthSystem()
    {
        return health;
    }

    public Rigidbody2D GetRigidbody()
    {
        return enemyRB;
    }

    public AudioClip GetShootSFX()
    {
        return shootSFX;
    }

    public bool GetSFXVariance()
    {
        return sfxVariance;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void SetMoveDirection(Vector2 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    public void ToggleMovement(bool toggle)
    {
        moving = toggle;
    }

    public void ToggleCollider(bool toggle)
    {
        bodyCollider.enabled = toggle;
    }

    public void ToggleVisual(bool toggle)
    {
        enemyVisual.SetActive(toggle);
    }

    public override void ToggleInactive(bool toggle)
    {
        ToggleCollider(!toggle);
        ToggleVisual(!toggle);
    }
}
