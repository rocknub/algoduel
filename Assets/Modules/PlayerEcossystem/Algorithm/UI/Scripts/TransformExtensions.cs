using UnityEngine;

public static class TransformExtensions
{
    public static int ActiveChildCount(this Transform transform)
    {
        var childCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i))
                childCount++;
        }
        return childCount;
    }
}