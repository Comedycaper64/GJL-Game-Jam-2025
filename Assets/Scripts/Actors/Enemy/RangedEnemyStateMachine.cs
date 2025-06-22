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
    private float sfxVolume = 0.25f;

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

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        stats = GetComponent<RangedEnemyStats>();

        ToggleCollider(false);
        health.SetMaxHealth(stats.health);

        health.OnDeath += HealthSystem_OnDeath;
    }

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;

        ResetEnemy();

        //If feedback on sound effects has been given, modify sound effect
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
        health.OnDeath -= HealthSystem_OnDeath;
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            enemyRB.AddForce(moveDirection * 2f * stats.movementSpeed);
        }
    }

    public override void SpawnEnemy()
    {
        health.SetMaxHealth(stats.health);
        enemyVisual.SetActive(true);
        SwitchState(new RangedEnemySpawnState(this));
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        enemyVisual.SetActive(false);
    }

    public override void ToggleInactive(bool toggle)
    {
        ToggleCollider(!toggle);
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
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

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        SwitchState(new RangedEnemyDeadState(this));
        OnEnemyDeath?.Invoke();
    }
}
