using System;
using UnityEngine;

public class PlayerAbsorber : MonoBehaviour
{
    private bool playerDead = false;
    private bool absorberAvailable = false;
    private bool absorberActive = false;
    private float absorbMeter = 1f;
    private float meterDrainRate;
    private float meterReplenishRate;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Collider2D absorbCollider;

    [SerializeField]
    private GameObject absorbVisual;

    [SerializeField]
    private AudioClip absorbSFX;

    [SerializeField]
    private AudioClip absorbBuffSFX;

    public static EventHandler<bool> OnAbsorbToggle;

    private void Start()
    {
        playerStats = PlayerIdentifier.PlayerTransform.GetComponent<PlayerStats>();
        meterDrainRate = playerStats.meterDrainRate;
        meterReplenishRate = playerStats.meterReplenishRate;

        ToggleAbsorberAvailable(true);
        ToggleAbsorber(false);
        absorberAvailable = true;

        PlayerManager.OnPlayerDead += DisableAbsorber;
    }

    private void OnDisable()
    {
        InputManager.OnAbsorbEvent -= TryAbsorb;
        InputManager.OnAbsorbReleaseEvent -= EndAbsorb;
        PlayerManager.OnPlayerDead -= DisableAbsorber;
    }

    private void Update()
    {
        if (playerDead)
        {
            return;
        }

        if (absorberActive)
        {
            absorbMeter = Mathf.Clamp01(absorbMeter - (meterDrainRate * Time.deltaTime));

            if (absorbMeter <= 0f)
            {
                EndAbsorb();
            }
        }
        else
        {
            absorbMeter = Mathf.Clamp01(absorbMeter + (meterReplenishRate * Time.deltaTime));
        }
    }

    private void TryAbsorb()
    {
        if (playerDead)
        {
            return;
        }

        if (!absorberAvailable)
        {
            return;
        }

        ToggleAbsorber(true);

        //Play Absorb Effect
        AudioManager.PlaySFX(absorbSFX, 0.5f, 0, transform.position);
    }

    private void ToggleAbsorber(bool toggle)
    {
        absorbCollider.enabled = toggle;
        absorbVisual.SetActive(toggle);
        absorberActive = toggle;

        OnAbsorbToggle?.Invoke(this, toggle);
    }

    private void EndAbsorb()
    {
        ToggleAbsorber(false);
    }

    public void ToggleAbsorberAvailable(bool toggle)
    {
        if (toggle)
        {
            if (!absorberAvailable)
            {
                InputManager.OnAbsorbEvent += TryAbsorb;
                InputManager.OnAbsorbReleaseEvent += EndAbsorb;
                absorberAvailable = true;
            }
        }
        else
        {
            InputManager.OnAbsorbEvent -= TryAbsorb;
            InputManager.OnAbsorbReleaseEvent -= EndAbsorb;
            absorberAvailable = false;
        }
    }

    public void AbsorbProjectile()
    {
        Debug.Log("Absorb");
    }

    private void DisableAbsorber(object sender, bool toggle)
    {
        playerDead = toggle;
    }
}
