using System;

public class FeedbackEndGameButtonUI : FeedbackButtonUI
{
    public static EventHandler<DialogueCluster> OnGiveFinalFeedback;

    public override void StartFeedbackConversation()
    {
        OnGiveFinalFeedback?.Invoke(this, feedbackCluster);
    }
}
