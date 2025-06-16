using System;
using UnityEngine;

public class FeedbackUI : MonoBehaviour
{
    private bool feedbackColumnOpen = false;
    private bool conversationActive = false;

    [SerializeField]
    private CanvasGroupFader feedbackButtonFader;

    private CanvasGroup feedBackColumnGroup;

    [SerializeField]
    private CanvasGroupFader feedbackColumnFader;

    [SerializeField]
    private CanvasGroupFader feedbackBusyFader;

    public static EventHandler<DialogueCluster> OnFeedback;

    private void Awake()
    {
        FeedbackButtonUI.OnGiveFeedback += TryStartFeedback;
        InputManager.OnFeedbackEvent += ToggleFeedbackMenu;
        ConversationManager.OnConversationActive += OnConversationActive;

        feedBackColumnGroup = feedbackColumnFader.GetComponent<CanvasGroup>();
        ToggleColumnFader(false);
        ToggleBusyFader(false);
    }

    private void OnDisable()
    {
        FeedbackButtonUI.OnGiveFeedback -= TryStartFeedback;
        InputManager.OnFeedbackEvent -= ToggleFeedbackMenu;
        ConversationManager.OnConversationActive -= OnConversationActive;
    }

    public void ToggleButtonFader(bool toggle)
    {
        feedbackButtonFader.ToggleFade(toggle);
        feedbackButtonFader.ToggleBlockRaycasts(toggle);
    }

    public void ToggleColumnFader(bool toggle)
    {
        feedbackColumnFader.ToggleFade(toggle);
        feedbackColumnFader.ToggleBlockRaycasts(toggle);
    }

    public void ToggleBusyFader(bool toggle)
    {
        feedbackBusyFader.ToggleFade(toggle);
        feedbackBusyFader.ToggleBlockRaycasts(toggle);
    }

    public void ToggleFeedbackMenu()
    {
        feedbackColumnOpen = !feedbackColumnOpen;
        ToggleButtonFader(!feedbackColumnOpen);
        ToggleColumnFader(feedbackColumnOpen);
    }

    private void TryStartFeedback(object sender, DialogueCluster cluster)
    {
        if (conversationActive)
        {
            return;
        }

        //start conversation with cluster
        OnFeedback?.Invoke(this, cluster);

        FeedbackButtonUI feedbackButton = sender as FeedbackButtonUI;
        feedbackButton.DisableButton();
    }

    private void OnConversationActive(object sender, bool toggle)
    {
        conversationActive = toggle;

        ToggleBusyFader(conversationActive);
        feedBackColumnGroup.interactable = !conversationActive;
    }
}
