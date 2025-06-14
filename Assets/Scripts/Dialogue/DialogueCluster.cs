using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCluster", menuName = "Dialogue/Cluster", order = 0)]
public class CinematicSO : ScriptableObject
{
    [SerializeField]
    private CinematicNode[] cinematicNodes;

    public CinematicNode[] GetCinematicNodes()
    {
        return cinematicNodes;
    }
}
