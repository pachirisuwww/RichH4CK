using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NestedUtility
{
    public static void TransformTreeAction(Transform root, System.Action<Transform> action)
    {
        action.Invoke(root);
        foreach (Transform child in root)
        {
            action.Invoke(child);
            TransformTreeAction(child, action);
        }
    }
}
