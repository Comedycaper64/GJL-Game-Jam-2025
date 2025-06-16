using UnityEngine;

public class PlayerDeadUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader fader;

    private void Start()
    {
        PlayerManager.OnPlayerDead += OnPlayerDead;
        fader.SetCanvasGroupAlpha(0f);
        fader.ToggleBlockRaycasts(false);
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerDead -= OnPlayerDead;
    }

    private void OnPlayerDead(object sender, bool toggle)
    {
        fader.ToggleFade(toggle);
        fader.ToggleBlockRaycasts(toggle);
    }
}
