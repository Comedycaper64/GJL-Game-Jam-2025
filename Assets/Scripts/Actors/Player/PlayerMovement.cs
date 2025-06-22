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
    private float sfxVolume = 0.15f;

    [SerializeField]
    private Rigidbody2D playerRb;

    [SerializeField]
    private Transform visualTransform;

    [SerializeField]
    private AudioClip dashSFX;

    [SerializeField]
    private AudioClip altDashSFX;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        movementSpeed = stats.movementSpeed;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;

        ToggleCanMove(true);

        PlayerManager.OnPlayerDead += OnPlayerDead;

        //If feedback on sound effects has been given, modify sound effect
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
        if (canMove)
        {
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
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveRB();
        }
    }

    private Vector2 MoveRB()
    {
        Vector2 movementValue = inputManager.MovementValue.normalized;
        playerRb.MovePosition(
            playerRb.position
                + new Vector2(movementValue.x, movementValue.y)
                    * movementSpeed
                    * dashModifier
                    * Time.fixedDeltaTime
        );

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
