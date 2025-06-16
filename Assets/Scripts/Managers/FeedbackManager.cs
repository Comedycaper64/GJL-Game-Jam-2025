using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
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
}
