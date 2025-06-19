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

        Debug.Log("ded");

        npcDialogue.StopDialogue();
        feedbackSender.SendFeedback();
        isDead = true;

        foreach (GameObject disableable in gameObjectsToDisable)
        {
            disableable.SetActive(false);
        }
    }
}
