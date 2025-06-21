using UnityEngine;

public class AttitudeManager : MonoBehaviour
{
    private int developerAttitude = 0;

    private const int STARTING_ATTITUDE = 15;

    public static AttitudeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "There's more than one AttitudeManager! " + transform + " - " + Instance
            );
            Destroy(gameObject);
            return;
        }
        Instance = this;

        transform.SetParent(null);

        SetStartingAttitude();

        DontDestroyOnLoad(gameObject);
    }

    public int GetAttitude()
    {
        return developerAttitude;
    }

    public void SetStartingAttitude()
    {
        developerAttitude = STARTING_ATTITUDE;
    }

    public void AlterAttitude(int attitudeModifier)
    {
        developerAttitude += attitudeModifier;
    }
}
