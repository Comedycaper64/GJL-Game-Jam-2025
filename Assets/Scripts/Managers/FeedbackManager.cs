using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum FeedbackType
{
    na,
    praise,
    critique,
    constructive,
    silence
}

public class FeedbackManager : MonoBehaviour
{
    private int praiseCount = 0;
    private int critiqueCount = 0;
    private int constructiveCount = 0;
    private int silenceCount = 0;

    private Dictionary<string, int> feedbackDictionary = new Dictionary<string, int>();

    public static FeedbackManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(
                "There's more than one FeedbackManager! " + transform + " - " + Instance
            );
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // DON'T DESTROY ON LOAD
    }

    private void OnEnable()
    {
        DialogueChoiceManager.OnFeedbackType += UpdateFeedbackCounts;
    }

    private void OnDisable()
    {
        DialogueChoiceManager.OnFeedbackType -= UpdateFeedbackCounts;
    }

    public void SetDictionaryValue(string key, int value)
    {
        if (!feedbackDictionary.TryAdd(key, value))
        {
            feedbackDictionary[key] = value;
        }
    }

    public bool TryGetDictionaryValue(string key, out int value)
    {
        if (feedbackDictionary.TryGetValue(key, out int val))
        {
            value = val;
            return true;
        }

        value = 0;
        return false;
    }

    private void UpdateFeedbackCounts(object sender, FeedbackType type)
    {
        switch (type)
        {
            case FeedbackType.praise:
                praiseCount++;
                break;
            case FeedbackType.constructive:
                constructiveCount++;
                break;
            case FeedbackType.critique:
                critiqueCount++;
                break;
            case FeedbackType.silence:
                Debug.Log("Silence Feedback");
                silenceCount++;
                break;

            default:
                break;
        }
    }
}
