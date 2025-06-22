using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(
    fileName = "DialogueConditional",
    menuName = "Dialogue/DialogueConditionalSO",
    order = 2
)]
public class DialogueConditionalSO : ConversationNode
{
    [SerializeField]
    private int attitudeThreshold = -1;

    [SerializeField]
    private string feedbackKey = "";

    [SerializeField]
    private DialogueCluster[] feedbackClusters;

    [SerializeField]
    private DialogueCluster noFeedbackCluster;

    [SerializeField]
    private DialogueCluster attitudeBranchCluster;

    //BRANCH CLUSTER IS RETURNED IF
    //  FEEDBACK VALUE MATCHES KEY-RETRIEVED FEEDBACK VALUE
    //  ATTITUDE IS EQUAL OR GREATER THAN ATTITUDE THRESHOLD

    public bool EvaluateKey(out string key)
    {
        key = feedbackKey;

        if (feedbackKey == "")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public ConversationNode[] GetConversation(int attitude = -1, int feedbackValue = -1)
    {
        if (attitudeThreshold < 0)
        {
            if ((feedbackValue < 0) || (feedbackValue >= feedbackClusters.Length))
            {
                if (noFeedbackCluster != null)
                {
                    return noFeedbackCluster.GetCinematicNodes();
                }
                else
                {
                    return feedbackClusters[0].GetCinematicNodes();
                }
            }

            return feedbackClusters[feedbackValue].GetCinematicNodes();
        }

        if (attitudeThreshold <= attitude)
        {
            return attitudeBranchCluster.GetCinematicNodes();
        }

        return feedbackClusters[0].GetCinematicNodes();
    }
}
