using UnityEngine;

public class PathHolder : MonoBehaviour
{
    public Transform[] points;

    private void Awake()
    {
        points = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}
