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
    private DialogueChoiceManager dialogueChoiceManager;

    public static EventHandler<bool> OnConversationActive;

    private void Awake()
    {
        AreaEventTrigger.OnAreaEvent += ConversationTrigger;
        FeedbackUI.OnFeedback += ConversationTrigger;
    }

    private void Start()
    {
        dialogueChoiceManager = dialogueManager.GetComponent<DialogueChoiceManager>();

        AddToConversation(startingCluster);
    }

    private void OnDisable()
    {
        AreaEventTrigger.OnAreaEvent -= ConversationTrigger;
        FeedbackUI.OnFeedback -= ConversationTrigger;
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
            else if (nodeType == typeof(DialogueChoiceSO))
            {
                dialogueChoiceManager.DisplayChoices(
                    node as DialogueChoiceSO,
                    TryResolveDialogueCluster
                );
            }
            else if (nodeType == typeof(DialogueConditionalSO))
            {
                DialogueConditionalSO conditionalSO = node as DialogueConditionalSO;

                ResolveConditional(conditionalSO);

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

    private void ResolveConditional(DialogueConditionalSO conditionalSO)
    {
        int feedbackValue = -1;

        if (conditionalSO.EvaluateKey(out string key))
        {
            // Get feedback value from feedback manager
            FeedbackManager.Instance.TryGetDictionaryValue(key, out feedbackValue);
        }

        // Debug.Log("Attitude: " + AttitudeManager.Instance.GetAttitude());

        // Debug.Log("Feedback value: " + feedbackValue);
        // Debug.Log(
        //     "Conversation Length: "
        //         + conditionalSO
        //             .GetConversation(AttitudeManager.Instance.GetAttitude(), feedbackValue)
        //             .Length
        // );

        ConversationNode[] newConversation = conditionalSO.GetConversation(
            AttitudeManager.Instance.GetAttitude(),
            feedbackValue
        );

        //Adds in new conversation before rest of cluster

        ConversationNode[] concatConversaiton = newConversation
            .Concat(activeDialogueCluster)
            .ToArray();
        activeDialogueCluster = new Queue<ConversationNode>(concatConversaiton);
    }

    private void EndConversation()
    {
        conversationActive = false;
        OnConversationActive?.Invoke(this, false);
    }

    public void AddToConversation(DialogueCluster newCluster)
    {
        conversationQueue.Enqueue(newCluster);

        if (!conversationActive)
        {
            AdvanceConversation();
            conversationActive = true;
            OnConversationActive?.Invoke(this, true);
        }
    }

    private void ConversationTrigger(object sender, DialogueCluster cluster)
    {
        AddToConversation(cluster);
    }
}
