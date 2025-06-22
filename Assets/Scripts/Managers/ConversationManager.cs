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
    private DialogueManager dialogueManager;
    private DialogueChoiceManager dialogueChoiceManager;

    private Action OnClusterEnd;

    public static EventHandler<bool> OnConversationActive;

    public static EventHandler<int> OnUnlockFeedback;

    public static Action OnPlaytestEnd;

    private void Awake()
    {
        AreaEventTrigger.OnAreaEvent += ConversationTrigger;
        FeedbackUI.OnFeedback += ConversationTrigger;
        FeedbackUI.OnFinalFeedback += EndConversationTrigger;
        CombatArenaManager.OnCombatEnd += ConversationTrigger;
        FeedbackSender.OnSendFeedback += ConversationTrigger;
    }

    private void Start()
    {
        dialogueChoiceManager = dialogueManager.GetComponent<DialogueChoiceManager>();
    }

    private void OnDisable()
    {
        AreaEventTrigger.OnAreaEvent -= ConversationTrigger;
        FeedbackUI.OnFeedback -= ConversationTrigger;
        FeedbackUI.OnFinalFeedback -= EndConversationTrigger;
        CombatArenaManager.OnCombatEnd -= ConversationTrigger;
        FeedbackSender.OnSendFeedback -= ConversationTrigger;
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
            else if (nodeType == typeof(DialogueFeedbackSO))
            {
                DialogueFeedbackSO feedbackSO = node as DialogueFeedbackSO;

                FeedbackManager.Instance.SetDictionaryValue(
                    feedbackSO.GetKey(),
                    feedbackSO.GetValue()
                );

                TryResolveDialogueCluster();
            }
            else if (nodeType == typeof(DialogueFeedbackUnlockSO))
            {
                DialogueFeedbackUnlockSO unlockSO = node as DialogueFeedbackUnlockSO;

                OnUnlockFeedback?.Invoke(this, unlockSO.feedbackUnlockIndex);

                TryResolveDialogueCluster();
            }
            else
            {
                TryResolveDialogueCluster();
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
            FeedbackManager.Instance.TryGetDictionaryValue(key, out feedbackValue);
        }

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

        if (OnClusterEnd != null)
        {
            OnClusterEnd();
            OnClusterEnd = null;
        }
    }

    private void EndLevel()
    {
        InputManager.Instance.GameEnd();

        OnPlaytestEnd?.Invoke();
    }

    public void AddToConversation(DialogueCluster newCluster, Action onClusterEnd = null)
    {
        conversationQueue.Enqueue(newCluster);

        OnClusterEnd = onClusterEnd;

        if (!conversationActive)
        {
            conversationActive = true;
            OnConversationActive?.Invoke(this, true);
            AdvanceConversation();
        }
    }

    private void ConversationTrigger(object sender, DialogueCluster cluster)
    {
        AddToConversation(cluster);
    }

    private void EndConversationTrigger(object sender, DialogueCluster cluster)
    {
        FeedbackManager.Instance.SilenceTest();

        AddToConversation(cluster, EndLevel);
    }
}
