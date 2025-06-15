using UnityEngine;
using UnityEngine.UI;

public class PlayerAbsorbUI : MonoBehaviour
{
    private bool absorbActive = false;
    private float meterDrainRate;
    private float meterReplenishRate;
    private PlayerStats playerStats;

    [SerializeField]
    private Slider absorbMeter;

    private void Awake()
    {
        PlayerAbsorber.OnAbsorbToggle += ToggleAbsorb;

        absorbMeter.value = 1f;
    }

    private void Start()
    {
        playerStats = PlayerIdentifier.PlayerTransform.GetComponent<PlayerStats>();
        meterDrainRate = playerStats.meterDrainRate;
        meterReplenishRate = playerStats.meterReplenishRate;
    }

    private void OnDisable()
    {
        PlayerAbsorber.OnAbsorbToggle -= ToggleAbsorb;
    }

    private void Update()
    {
        float meterModifier = meterReplenishRate;
        if (absorbActive)
        {
            meterModifier = -meterDrainRate;
        }

        absorbMeter.value = Mathf.Clamp01(absorbMeter.value + (meterModifier * Time.deltaTime));
    }

    private void ToggleAbsorb(object sender, bool toggle)
    {
        absorbActive = toggle;
    }
}
