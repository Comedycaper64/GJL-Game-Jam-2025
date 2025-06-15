using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCluster", menuName = "Dialogue/Cluster", order = 0)]
public class DialogueCluster : ScriptableObject
{
    [SerializeField]
    private ConversationNode[] conversationNodes;

    public ConversationNode[] GetCinematicNodes()
    {
        return conversationNodes;
    }
}
