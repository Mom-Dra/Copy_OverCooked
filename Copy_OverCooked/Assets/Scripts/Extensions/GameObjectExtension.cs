using UnityEngine;

public enum EDebugColor
{
    Red,
    Yellow,
    Orange,
    Blue
}

public static class GameObjectExtension
{
    public static void DebugName(this GameObject gameObject, string prefix = "", EDebugColor eDebugColor = EDebugColor.Yellow)
    {
        Debug.Log($"<color={eDebugColor.ToString().ToLower()}> {prefix} : {gameObject.name} </color>");
    }
}