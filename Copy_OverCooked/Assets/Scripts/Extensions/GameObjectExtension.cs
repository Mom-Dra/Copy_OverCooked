using UnityEngine;

public enum EDebugColor
{
    Red,
    Yellow
}

public static class GameObjectExtension
{
    public static void DebugName(this GameObject gameObject, EDebugColor eDebugColor = EDebugColor.Yellow, string prefix = "")
    {
        Debug.Log($"<color={eDebugColor.ToString().ToLower()}> {prefix}: {gameObject.name} </color>");
    }
}