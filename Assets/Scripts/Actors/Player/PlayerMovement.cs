using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    private Transform visualTransform;

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
    }

    private void OnDisable()
    {
        InputManager.OnDashEvent -= TryDash;

        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
    }

    private void Update()
    {
        Vector2 movementValue = Vector2.zero;
        if (canMove)
        {
            movementValue = Move();

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

    private void TryDash()
    {
        if (!dashAvailable)
        {
            return;
        }

        dashCoroutine = StartCoroutine(ApplyDash());

        //Dash Effect
        //Dash SFX

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
}
