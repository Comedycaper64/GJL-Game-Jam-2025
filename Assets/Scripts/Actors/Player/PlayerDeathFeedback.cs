using UnityEngine;

public class PlayerDeathFeedback : MonoBehaviour
{
    private int deathCounter = 0;

    [SerializeField]
    private FeedbackSender feedbackSender;

    [SerializeField]
    private DialogueCluster[] dialogueClusters;

    private void OnEnable()
    {
        PlayerManager.OnPlayerDead += SendFeedback;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerDead -= SendFeedback;
    }

    private void SendFeedback(object sender, bool dead)
    {
        if (!dead)
        {
            return;
        }

        if (deathCounter >= dialogueClusters.Length)
        {
            return;
        }

        feedbackSender.SendFeedback(dialogueClusters[deathCounter]);

        deathCounter++;
    }
}
