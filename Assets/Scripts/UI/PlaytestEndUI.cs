using UnityEngine;

public class PlaytestEndUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader fader;

    private void Start()
    {
        ConversationManager.OnPlaytestEnd += StartPlaytestEndUI;
    }

    private void OnDisable()
    {
        ConversationManager.OnPlaytestEnd -= StartPlaytestEndUI;
    }

    private void StartPlaytestEndUI()
    {
        fader.ToggleFade(true);
        //Evaluate player choices
        //Start End Timeline
    }
}
