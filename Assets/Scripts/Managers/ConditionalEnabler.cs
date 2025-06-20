using UnityEngine;

public class ConditionalEnabler : MonoBehaviour
{
    [SerializeField]
    private bool evaluateOnStart = false;

    [SerializeField]
    private bool enable = true;

    [SerializeField]
    private int attitudeThreshold = -1;

    [SerializeField]
    private string feedbackKey = "";

    [SerializeField]
    private int feedbackValue = -1;

    [SerializeField]
    private GameObject falseEnable;

    [SerializeField]
    private GameObject trueEnable;

    private void Start()
    {
        if (evaluateOnStart)
        {
            EvaluateCondition();
        }
    }

    public void EvaluateCondition()
    {
        if (attitudeThreshold < 0)
        {
            if (
                FeedbackManager.Instance.TryGetDictionaryValue(feedbackKey, out int value)
                && (value == feedbackValue)
            )
            {
                TryTrueEnable();
            }
            else
            {
                TryFalseEnable();
            }
        }

        if (attitudeThreshold <= AttitudeManager.Instance.GetAttitude())
        {
            TryTrueEnable();
        }

        TryFalseEnable();
    }

    private void TryTrueEnable()
    {
        if (trueEnable != null)
        {
            trueEnable.SetActive(enable);
        }
    }

    private void TryFalseEnable()
    {
        if (falseEnable != null)
        {
            falseEnable.SetActive(enable);
        }
    }
}
