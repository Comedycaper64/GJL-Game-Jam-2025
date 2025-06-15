using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    private bool conversationActive = false;
    private Queue<ConversationNode> activeDialogueCluster = new Queue<ConversationNode>();
    private Queue<DialogueCluster> conversationQueue = new Queue<DialogueCluster>();

    [SerializeField]
    private DialogueCluster startingCluster;

    [SerializeField]
    private DialogueManager dialogueManager;

    private void Awake()
    {
        AreaEventTrigger.OnAreaEvent += ConversationTrigger;
    }

    private void Start()
    {
        AddToConversation(startingCluster);
    }

    private void OnDisable()
    {
        AreaEventTrigger.OnAreaEvent -= ConversationTrigger;
    }

    private void AdvanceConversation()
    {
        if (conversationQueue.TryDequeue(out DialogueCluster newCluster))
        {
            activeDialogueCluster = new Queue<ConversationNode>(newCluster.GetCinematicNodes());
            TryResolveDialogueCluster();
        }
        else
        {
            EndConversation();
        }
    }

    private void TryResolveDialogueCluster()
    {
        if (activeDialogueCluster.TryDequeue(out ConversationNode node))
        {
            Type nodeType = node.GetType();

            if (nodeType == typeof(DialogueSO))
            {
                dialogueManager.PlayDialogue(node as DialogueSO, TryResolveDialogueCluster);
            }
            else if (nodeType == typeof(DialogueConditionalSO))
            {
                DialogueConditionalSO conditionalSO = node as DialogueConditionalSO;
                int feedbackValue = -1;

                if (conditionalSO.EvaluateKey(out string key))
                {
                    // Get feedback value from feedback manager
                    FeedbackManager.Instance.TryGetDictionaryValue(key, out feedbackValue);
                }

                Debug.Log("Attitude: " + AttitudeManager.Instance.GetAttitude());

                Debug.Log("Feedback value: " + feedbackValue);
                Debug.Log(
                    "Conversation Length: "
                        + conditionalSO
                            .GetConversation(AttitudeManager.Instance.GetAttitude(), feedbackValue)
                            .Length
                );

                ConversationNode[] newConversation = conditionalSO.GetConversation(
                    AttitudeManager.Instance.GetAttitude(),
                    feedbackValue
                );

                //Adds in new conversation before rest of cluster

                ConversationNode[] concatConversaiton = newConversation
                    .Concat(activeDialogueCluster)
                    .ToArray();
                activeDialogueCluster = new Queue<ConversationNode>(concatConversaiton);

                TryResolveDialogueCluster();
            }
            else
            {
                TryResolveDialogueCluster();
                Debug.Log("Error, undefined type");
            }
        }
        else
        {
            AdvanceConversation();
        }
    }

    private void EndConversation()
    {
        conversationActive = false;
    }

    public void AddToConversation(DialogueCluster newCluster)
    {
        conversationQueue.Enqueue(newCluster);

        if (!conversationActive)
        {
            AdvanceConversation();
            conversationActive = true;
        }
    }

    private void ConversationTrigger(object sender, DialogueCluster cluster)
    {
        AddToConversation(cluster);
    }
}
