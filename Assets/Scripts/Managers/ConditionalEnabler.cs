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
    private GameObject[] falseEnable;

    [SerializeField]
    private GameObject[] trueEnable;

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
                return;
            }
            else
            {
                TryFalseEnable();
                return;
            }
        }

        if (attitudeThreshold <= AttitudeManager.Instance.GetAttitude())
        {
            TryTrueEnable();
            return;
        }

        TryFalseEnable();
    }

    private void TryTrueEnable()
    {
        if (trueEnable != null)
        {
            foreach (GameObject gameObject in trueEnable)
            {
                gameObject.SetActive(enable);
            }
        }
    }

    private void TryFalseEnable()
    {
        if (falseEnable != null)
        {
            foreach (GameObject gameObject in falseEnable)
            {
                gameObject.SetActive(enable);
            }
        }
    }
}
