using UnityEngine;

public class DebugFeedbackSetter : MonoBehaviour
{
    public void SetFeedback()
    {
        FeedbackManager.Instance.SetDictionaryValue("Music", 2);
        FeedbackManager.Instance.SetDictionaryValue("SFX", 1);
        FeedbackManager.Instance.SetDictionaryValue("Speed", 1);
        FeedbackManager.Instance.SetDictionaryValue("Writing", 0);
        FeedbackManager.Instance.SetDictionaryValue("Attack", 1);
        FeedbackManager.Instance.SetDictionaryValue("Diff", 1);
        FeedbackManager.Instance.SetDictionaryValue("Absorb", 0);
        FeedbackManager.Instance.SetDictionaryValue("Style", 0);
        //FeedbackManager.Instance.SetDictionaryValue("Music", 0);
    }
}
