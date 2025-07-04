using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackButtonUI : MonoBehaviour
{
    [SerializeField]
    protected DialogueCluster feedbackCluster;

    [SerializeField]
    private Button feedbackButton;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    public static EventHandler<DialogueCluster> OnGiveFeedback;

    public virtual void StartFeedbackConversation()
    {
        OnGiveFeedback?.Invoke(this, feedbackCluster);
    }

    public void DisableButton()
    {
        feedbackButton.interactable = false;
        buttonText.color = Color.gray;
    }
}
