using UnityEngine;

public class Utill
{
    public static void AddComponentToChild(GameObject parent, GameObject child)
    {
        child.transform.SetParent(parent.transform);
    }
}
