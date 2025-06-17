using UnityEngine;
using UnityEngine.UI;

public class MessengerManager : MonoBehaviour
{
    [SerializeField]
    private DialogueCluster openingDialogueCluster;

    [SerializeField]
    private ConversationManager conversationManager;

    [SerializeField]
    private DialogueUI dialogueUI;

    [SerializeField]
    private Button callButton;

    [SerializeField]
    private CanvasGroupFader preCallFader;

    [SerializeField]
    private CanvasGroupFader callStartFader;

    [SerializeField]
    private CanvasGroupFader messengerUIFader;

    public void StartCall()
    {
        dialogueUI.StartCallTimer();
        conversationManager.AddToConversation(openingDialogueCluster, StartGame);

        //augment UI so that player had joined the call
        //disable call button
        callButton.interactable = false;
        preCallFader.ToggleFade(false);
        preCallFader.ToggleBlockRaycasts(false);
        callStartFader.ToggleFade(true);
    }

    public void StartGame()
    {
        messengerUIFader.ToggleFade(false);
        messengerUIFader.ToggleBlockRaycasts(false);
        InputManager.Instance.GameStart();

        //fadeout messenger UI
        //enable character movement + inputs
    }
}
