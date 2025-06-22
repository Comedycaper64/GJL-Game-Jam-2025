using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    private HealthSystem playerHealth;

    [SerializeField]
    private GameObject[] healthIcons;

    private void Start()
    {
        playerHealth = PlayerIdentifier.PlayerTransform.GetComponent<HealthSystem>();

        playerHealth.OnNewHealth += UpdateHealth;
    }

    private void OnDisable()
    {
        playerHealth.OnNewHealth -= UpdateHealth;
    }

    private void ResetHealthUI()
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].SetActive(false);
        }
    }

    private void UpdateHealth(object sender, int newHealth)
    {
        ResetHealthUI();

        for (int i = 0; i < newHealth; i++)
        {
            healthIcons[i].SetActive(true);
        }
    }
}
