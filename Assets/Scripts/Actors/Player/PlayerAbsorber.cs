using System;
using System.Collections;
using UnityEngine;

public class PlayerAbsorber : MonoBehaviour
{
    private bool sfxVariance = false;
    private bool playerDead = false;
    private bool absorberAvailable = false;
    private bool absorberActive = false;
    private int absorbType = -1;
    private int projectilesAbsorbed = 0;
    private float absorbMeter = 1f;
    private float meterDrainRate;
    private float meterReplenishRate;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private HealthSystem playerHealth;

    [SerializeField]
    private Collider2D absorbCollider;

    [SerializeField]
    private PlayerCursorPointer pointer;

    [SerializeField]
    private GameObject absorbVisual;

    [SerializeField]
    private GameObject buffVisual;

    [SerializeField]
    private AudioClip absorbSFX;

    [SerializeField]
    private AudioClip altAbsorbSFX;

    [SerializeField]
    private AudioClip absorbBuffSFX;

    [SerializeField]
    private AudioClip altAbsorbBuffSFX;

    [SerializeField]
    private float sfxVolume = 0.25f;

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

        //If feedback on sound effects has been given, modify sound effect
        if (FeedbackManager.Instance.TryGetDictionaryValue("SFX", out int val))
        {
            if (val == 1)
            {
                sfxVariance = true;
                absorbBuffSFX = altAbsorbBuffSFX;
                absorbSFX = altAbsorbSFX;
            }
            else if (val == 2)
            {
                sfxVolume = 0f;
            }
        }

        //If feedback on absorb has been given, modify active absorb ability
        if (FeedbackManager.Instance.TryGetDictionaryValue("Absorb", out int val2))
        {
            absorbType = val2;
        }
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

        AudioManager.PlaySFX(absorbSFX, sfxVolume, 0, transform.position, sfxVariance);
    }

    private void ToggleAbsorber(bool toggle)
    {
        if (toggle)
        {
            projectilesAbsorbed = 0;
        }

        absorbCollider.enabled = toggle;
        absorbVisual.SetActive(toggle);
        absorberActive = toggle;

        OnAbsorbToggle?.Invoke(this, toggle);
    }

    private void EndAbsorb()
    {
        ToggleAbsorber(false);

        if ((absorbType >= 0) && (projectilesAbsorbed > 0))
        {
            ActivateAbsorbAbility(absorbType);
        }
    }

    private void ActivateAbsorbAbility(int abilityType)
    {
        switch (abilityType)
        {
            case 0:
                HealPlayer();
                break;
            case 1:
                BoostDamage();
                break;
            case 2:
                LaunchProjectile();
                break;
        }
    }

    private void LaunchProjectile()
    {
        ProjectileManager.SpawnProjectile(
            transform.position,
            pointer.GetCursorDirection(),
            projectilesAbsorbed,
            6 * projectilesAbsorbed,
            true
        );
    }

    private void BoostDamage()
    {
        playerStats.damageBuff = projectilesAbsorbed;
        AudioManager.PlaySFX(absorbBuffSFX, sfxVolume, 0, transform.position, sfxVariance);
        StartCoroutine(BuffEnd());
        buffVisual.SetActive(true);
    }

    private IEnumerator BuffEnd()
    {
        yield return new WaitForSeconds(projectilesAbsorbed);
        buffVisual.SetActive(false);
        playerStats.damageBuff = 0;
    }

    private void HealPlayer()
    {
        playerHealth.Heal(projectilesAbsorbed);
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
        projectilesAbsorbed++;
    }

    private void DisableAbsorber(object sender, bool toggle)
    {
        playerDead = toggle;
    }
}
