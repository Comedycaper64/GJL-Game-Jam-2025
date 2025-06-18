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

    [SerializeField]
    private GameObject[] unlockableFeedbacks;

    public static EventHandler<DialogueCluster> OnFeedback;
    public static EventHandler<DialogueCluster> OnFinalFeedback;

    private void Awake()
    {
        FeedbackButtonUI.OnGiveFeedback += TryStartFeedback;
        FeedbackEndGameButtonUI.OnGiveFinalFeedback += TryStartFinalFeedback;
        InputManager.OnFeedbackEvent += ToggleFeedbackMenu;
        ConversationManager.OnConversationActive += OnConversationActive;
        ConversationManager.OnUnlockFeedback += OnUnlockFeedback;

        feedBackColumnGroup = feedbackColumnFader.GetComponent<CanvasGroup>();
        ToggleColumnFader(false);
        ToggleBusyFader(false);
    }

    private void OnDisable()
    {
        FeedbackButtonUI.OnGiveFeedback -= TryStartFeedback;
        FeedbackEndGameButtonUI.OnGiveFinalFeedback -= TryStartFinalFeedback;
        InputManager.OnFeedbackEvent -= ToggleFeedbackMenu;
        ConversationManager.OnConversationActive -= OnConversationActive;
        ConversationManager.OnUnlockFeedback -= OnUnlockFeedback;
    }

    private void UpdateFeedbackUI()
    {
        //update feedback Ui number
        //flash Ui
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

    private void TryStartFinalFeedback(object sender, DialogueCluster cluster)
    {
        if (conversationActive)
        {
            return;
        }

        OnFinalFeedback?.Invoke(this, cluster);

        FeedbackButtonUI feedbackButton = sender as FeedbackButtonUI;
        feedbackButton.DisableButton();
    }

    private void OnConversationActive(object sender, bool toggle)
    {
        conversationActive = toggle;

        ToggleBusyFader(conversationActive);
        feedBackColumnGroup.interactable = !conversationActive;
    }

    private void OnUnlockFeedback(object sender, int index)
    {
        unlockableFeedbacks[index].SetActive(true);
        UpdateFeedbackUI();
    }
}
