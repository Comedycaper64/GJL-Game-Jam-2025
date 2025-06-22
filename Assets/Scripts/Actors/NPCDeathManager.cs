using System;
using UnityEngine;

public class NPCDeathManager : MonoBehaviour
{
    private bool isDead = false;

    [SerializeField]
    private HealthSystem npcHealth;

    [SerializeField]
    private NPCDialogue npcDialogue;

    [SerializeField]
    private FeedbackSender feedbackSender;

    [SerializeField]
    private GameObject[] gameObjectsToDisable;

    private void Start()
    {
        if (FeedbackManager.Instance.TryGetDictionaryValue("NPC_INV", out int val))
        {
            if (val == 1)
            {
                npcHealth.SetInvincible(true);
            }
        }
    }

    private void OnEnable()
    {
        npcHealth.OnDeath += NPCDeath;
    }

    private void OnDisable()
    {
        npcHealth.OnDeath -= NPCDeath;
    }

    private void NPCDeath(object sender, EventArgs e)
    {
        if (isDead)
        {
            return;
        }

        npcDialogue.StopDialogue();
        feedbackSender.SendFeedback();
        isDead = true;

        foreach (GameObject disableable in gameObjectsToDisable)
        {
            disableable.SetActive(false);
        }
    }
}
