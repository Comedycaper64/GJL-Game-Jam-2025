using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class MessengerManager : MonoBehaviour
{
    private float loadingScreenDisplayTime = 4f;
    private float logoRotateSpeed = 100f;

    [SerializeField]
    private DialogueCluster openingDialogueCluster;

    [SerializeField]
    private DialogueCluster gameStartCluster;

    [SerializeField]
    private Transform logoTransform;

    [SerializeField]
    private ConversationManager conversationManager;

    [SerializeField]
    private DialogueUI dialogueUI;

    [SerializeField]
    private Button callButton;

    [SerializeField]
    private MMF_Player feedbackUIPlayer;

    [SerializeField]
    private Scrollbar messengerScrollbar;

    [SerializeField]
    private CanvasGroupFader loadFader;

    [SerializeField]
    private CanvasGroupFader preCallFader;

    [SerializeField]
    private CanvasGroupFader callStartFader;

    [SerializeField]
    private CanvasGroupFader darknessFader;

    [SerializeField]
    private CanvasGroupFader messengerUIFader;

    [SerializeField]
    private AudioManager audioManager;

    private void Awake()
    {
        messengerScrollbar.value = 0f;
        loadFader.SetCanvasGroupAlpha(1f);
        messengerUIFader.SetCanvasGroupAlpha(1f);

        StartCoroutine(DelayedLoadDisable());
    }

    private IEnumerator DelayedLoadDisable()
    {
        yield return new WaitForSeconds(loadingScreenDisplayTime);

        loadFader.ToggleFade(false);
        loadFader.ToggleBlockRaycasts(false);
    }

    private IEnumerator DelayedGameLoad()
    {
        darknessFader.ToggleFade(true);

        yield return new WaitForSeconds(1f);
        messengerUIFader.ToggleFade(false);
        messengerUIFader.ToggleBlockRaycasts(false);

        yield return new WaitForSeconds(1f);
        audioManager.StartMusic();

        darknessFader.ToggleFade(false);

        InputManager.Instance.GameStart();
        conversationManager.AddToConversation(gameStartCluster);

        yield return new WaitForSeconds(26f);

        feedbackUIPlayer.PlayFeedbacks();
        //another yield
        //flash feedback button when dialogue gets to it
    }

    private void Update()
    {
        logoTransform.eulerAngles += new Vector3(0f, logoRotateSpeed * Time.deltaTime, 0f);
    }

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
        FeedbackManager.Instance.ResetFeedbackTypes();

        StartCoroutine(DelayedGameLoad());
    }
}
