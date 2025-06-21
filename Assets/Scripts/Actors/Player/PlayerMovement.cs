using System;
using System.Collections;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool sfxVariance = false;
    private bool canMove;
    private bool dashAvailable;
    private float movementSpeed;
    private float dashModifier = 1f;
    private float dashTimer = 0f;
    private PlayerStats stats;
    private Coroutine dashCoroutine;
    private InputManager inputManager;

    [SerializeField]
    private Animator bodyAnimator;

    [SerializeField]
    private Rigidbody2D playerRb;

    [SerializeField]
    private Transform visualTransform;

    [SerializeField]
    private AudioClip dashSFX;

    [SerializeField]
    private AudioClip altDashSFX;

    [SerializeField]
    private float sfxVolume = 0.25f;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        movementSpeed = stats.movementSpeed;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;

        //Temp
        ToggleCanMove(true);

        PlayerManager.OnPlayerDead += OnPlayerDead;

        if (FeedbackManager.Instance.TryGetDictionaryValue("SFX", out int val))
        {
            if (val == 1)
            {
                sfxVariance = true;
                dashSFX = altDashSFX;
            }
            else if (val == 2)
            {
                sfxVolume = 0;
            }
        }
    }

    private void OnDisable()
    {
        InputManager.OnDashEvent -= TryDash;

        PlayerManager.OnPlayerDead -= OnPlayerDead;

        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
    }

    private void Update()
    {
        //Vector2 movementValue = Vector2.zero;
        if (canMove)
        {
            //movementValue = Move();
            //movementValue = MoveRB();

            if (!dashAvailable)
            {
                dashTimer += Time.deltaTime;

                if (dashTimer >= stats.dashRechargeTime)
                {
                    dashAvailable = true;
                    dashTimer = 0f;
                }
            }
        }

        //bodyAnimator.SetFloat("Speed", Mathf.Abs(movementValue.x) + Mathf.Abs(movementValue.y));
    }

    private void FixedUpdate()
    {
        Vector2 movementValue = Vector2.zero;
        if (canMove)
        {
            //movementValue = Move();
            movementValue = MoveRB();
        }
    }

    private Vector2 Move()
    {
        Vector2 movementValue = inputManager.MovementValue.normalized;
        transform.position +=
            new Vector3(movementValue.x, movementValue.y)
            * movementSpeed
            * dashModifier
            * Time.deltaTime;

        if (movementValue.x < 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movementValue.x > 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 0, 0);
        }

        return movementValue;
    }

    private Vector2 MoveRB()
    {
        Vector2 movementValue = inputManager.MovementValue.normalized;
        playerRb.MovePosition(
            playerRb.position
                + new Vector2(movementValue.x, movementValue.y)
                    * movementSpeed
                    //* 2.5f
                    * dashModifier
                    * Time.fixedDeltaTime
        );

        // playerRb.linearVelocity = Vector2.ClampMagnitude(
        //     playerRb.linearVelocity,
        //     movementSpeed * 2.5f * dashModifier
        // );

        // float maximumVelocity =

        // playerRb.AddForce();

        if (movementValue.x < 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movementValue.x > 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 0, 0);
        }

        return movementValue;
    }

    private void TryDash()
    {
        if (!dashAvailable)
        {
            return;
        }

        dashCoroutine = StartCoroutine(ApplyDash());

        //Dash Effect

        AudioManager.PlaySFX(dashSFX, sfxVolume, 0, transform.position, sfxVariance);

        dashAvailable = false;
    }

    private IEnumerator ApplyDash()
    {
        dashModifier = stats.dashSpeedModifier;
        yield return new WaitForSeconds(stats.dashTime);
        dashModifier = 1f;
    }

    public void ToggleCanMove(bool enable)
    {
        if (enable)
        {
            if (!canMove)
            {
                InputManager.OnDashEvent += TryDash;
            }
        }
        else
        {
            InputManager.OnDashEvent -= TryDash;
        }

        canMove = enable;
    }

    private void OnPlayerDead(object sender, bool playerDead)
    {
        ToggleCanMove(!playerDead);
    }
}
