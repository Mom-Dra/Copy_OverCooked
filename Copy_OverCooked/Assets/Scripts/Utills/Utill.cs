using UnityEngine;

public enum EDebugColor
{
    Red,
    Yellow,
    Green,
    Orange
}

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

    public static void DebugName(this GameObject gameObject, string message="", EDebugColor color = EDebugColor.Yellow)
    {
        Debug.Log($"<color={color.ToString().ToLower()}>[{gameObject.name}] : {message} </color>");
    }

    public static void Fix(this InteractableObject interactableObject)
    {
        Rigidbody rigidbody = interactableObject.GetComponent<Rigidbody>();
        if (rigidbody != null && rigidbody.isKinematic == false)
        {
            rigidbody.isKinematic = true;
        }
    }

    public static void Free(this InteractableObject interactableObject)
    {
        Rigidbody rigidbody = interactableObject.GetComponent<Rigidbody>();
        if (rigidbody != null && rigidbody.isKinematic == true)
        {
            rigidbody.isKinematic = false;
        }
    }
}
