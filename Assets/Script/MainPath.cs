
using UnityEngine;
using System.Collections.Generic;

public class MainPath : MonoBehaviour
{
    [Tooltip("Tiles in board order (0 .. N-1). Populated automatically from children on Awake.")]
    public List<Transform> tiles = new List<Transform>();

    private void Awake()
    {
        tiles.Clear();
        for (int i = 0; i < transform.childCount; i++)
            tiles.Add(transform.GetChild(i));
    }

    
#if UNITY_EDITOR
    [ContextMenu("Populate Tiles From Children")]
    private void PopulateTilesFromChildren()
    {
        tiles.Clear();
        for (int i = 0; i < transform.childCount; i++)
            tiles.Add(transform.GetChild(i));
    }
#endif
}
