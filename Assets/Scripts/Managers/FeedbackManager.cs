using System;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<FeedbackType, int> feedbackTypeDicitonary =
        new Dictionary<FeedbackType, int>();

    [SerializeField]
    private DebugFeedbackSetter debugFeedbackSetter;

    public static FeedbackManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        transform.SetParent(null);

        DontDestroyOnLoad(gameObject);

        if (debugFeedbackSetter)
        {
            debugFeedbackSetter.SetFeedback();
        }
    }

    private void Start()
    {
        if (feedbackTypeDicitonary.Count <= 0)
        {
            feedbackTypeDicitonary.Add(FeedbackType.praise, 0);
            feedbackTypeDicitonary.Add(FeedbackType.critique, 0);
            feedbackTypeDicitonary.Add(FeedbackType.constructive, 0);
            feedbackTypeDicitonary.Add(FeedbackType.silence, 0);
        }
    }

    private void OnEnable()
    {
        DialogueChoiceManager.OnFeedbackType += UpdateFeedbackCounts;
    }

    private void OnDisable()
    {
        DialogueChoiceManager.OnFeedbackType -= UpdateFeedbackCounts;
    }

    public void ResetFeedbackTypes()
    {
        feedbackTypeDicitonary[FeedbackType.praise] = 0;
        feedbackTypeDicitonary[FeedbackType.critique] = 0;
        feedbackTypeDicitonary[FeedbackType.constructive] = 0;
        feedbackTypeDicitonary[FeedbackType.silence] = 0;
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

    public FeedbackType GetHighestFeedbackType()
    {
        FeedbackType highestFeedbackType = feedbackTypeDicitonary
            .Aggregate((x, y) => x.Value > y.Value ? x : y)
            .Key;

        return highestFeedbackType;
    }

    public int GetChangesInfluenced()
    {
        int changes = 0;

        foreach (var entry in feedbackDictionary)
        {
            if (entry.Key == "Silence")
            {
                continue;
            }

            if (entry.Value > 0)
            {
                changes++;
            }
        }

        return changes;
    }

    public void SilenceTest()
    {
        if (GetHighestFeedbackType() == FeedbackType.silence)
        {
            feedbackDictionary.TryAdd("Silence", 1);
        }
    }

    private void UpdateFeedbackCounts(object sender, FeedbackType type)
    {
        switch (type)
        {
            case FeedbackType.praise:
                praiseCount++;
                feedbackTypeDicitonary[type] = praiseCount;
                break;
            case FeedbackType.constructive:
                constructiveCount++;
                feedbackTypeDicitonary[type] = constructiveCount;
                break;
            case FeedbackType.critique:
                critiqueCount++;
                feedbackTypeDicitonary[type] = critiqueCount;
                break;
            case FeedbackType.silence:
                silenceCount++;
                feedbackTypeDicitonary[type] = silenceCount;
                break;

            default:
                break;
        }
    }
}
