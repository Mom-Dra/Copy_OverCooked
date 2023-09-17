using UnityEngine;

public static class Utill
{
    public static void AddComponentToChild(GameObject parent, GameObject child)
    {
        child.transform.SetParent(parent.transform);
    }

    public static T Load<T>(string path = "") where T : Object
    {
        return Resources.Load<T>($"Prefabs/{path}");
    }

    public static Vector3 convert(Vector3 orgPos)
    {
        return Camera.main.WorldToScreenPoint(orgPos);
    }
}
