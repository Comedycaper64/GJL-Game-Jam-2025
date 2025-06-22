using System;
using UnityEngine;

public class MeleeEnemyStateMachine : StateMachine
{
    private bool moving = false;
    private Vector2 moveDirection;
    private Vector2 attackDirection;
    private Transform playerTransform;
    private HealthSystem health;
    private MeleeEnemyStats stats;

    [SerializeField]
    private Collider2D bodyCollider;

    [SerializeField]
    private Rigidbody2D enemyRB;

    [SerializeField]
    private GameObject enemyVisual;

    [SerializeField]
    private EnemyWeapon enemyWeapon;

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        stats = GetComponent<MeleeEnemyStats>();

        ToggleCollider(false);
        health.SetMaxHealth(stats.health);

        health.OnDeath += HealthSystem_OnDeath;
    }

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;

        ResetEnemy();
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
        SwitchState(new MeleeEnemySpawnState(this));
    }

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        enemyVisual.SetActive(false);
    }

    public override void ToggleInactive(bool toggle)
    {
        ToggleCollider(!toggle);
        ToggleVisual(!toggle);
    }

    public void PlayAttackAnimation()
    {
        smAnimator.SetTrigger("attack");

        enemyWeapon.PlayAttackAnimation();
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    public Vector2 GetAttackDirection()
    {
        return attackDirection;
    }

    public MeleeEnemyStats GetStats()
    {
        return stats;
    }

    public Rigidbody2D GetRigidbody()
    {
        return enemyRB;
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
        enemyWeapon.ToggleWeaponVisual(toggle);
    }

    public void SetMoveDirection(Vector2 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    public void SetAttackDirection(Vector2 direction)
    {
        attackDirection = direction;

        // enemyWeapon.SetAttackDirection(direction);
        enemyWeapon.SetAttackDirection(playerTransform.position);
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        SwitchState(new MeleeEnemyDeadState(this));
        OnEnemyDeath?.Invoke();
    }
}
