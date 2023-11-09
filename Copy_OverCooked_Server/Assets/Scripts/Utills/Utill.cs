using System.Collections.Generic;
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
    private static float BLEND_RATIO = 0.5f;

    public static void AddComponentToChild(this GameObject parent, GameObject child)
    {
        child.transform.SetParent(parent.transform);
    }

    public static T Load<T>(string path = "") where T : Object
    {
        return Resources.Load<T>($"Prefabs/{path}");
    }

    public static void DebugName(this GameObject gameObject, string message = "", EDebugColor color = EDebugColor.Yellow)
    {
        if(SettingManager.Instance.logEnabled)
        {
            Debug.Log($"<color={color.ToString().ToLower()}>[{gameObject.name}] : {message} </color>");
        }
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

    public static string SetAnimationClip(this GameObject gameObject, AnimationClip animationClip)
    {
        if (!gameObject.TryGetComponent<Animation>(out Animation anim))
        {
            anim = gameObject.AddComponent<Animation>();
        } 

        string clipName = animationClip.ToString();
        anim.AddClip(animationClip, clipName);
        return clipName;
    }

    public static Color SubtractiveMixColor(this Color color1, Color color2)
    {
        Color blendedColor = new Color(
            Mathf.Lerp(color1.r, color2.r, BLEND_RATIO),
            Mathf.Lerp(color1.g, color2.g, BLEND_RATIO),
            Mathf.Lerp(color1.b, color2.b, BLEND_RATIO),
            Mathf.Lerp(color1.a, color2.a, BLEND_RATIO)
        );
        return blendedColor;
    }

    public static List<T> GetComponentsInAllChildren<T>(this GameObject parent) where T : Component
    {
        List<T> componentList = new List<T> ();
        if(parent.TryGetComponent<T>(out T component))
        {
            componentList.Add(component);
        }

        if(parent.transform.childCount > 0)
        {
            GameObject child = null;
            for(int i = 0; i < parent.transform.childCount; i++)
            {
                child = parent.transform.GetChild(i).gameObject;
                componentList.AddRange(child.GetComponentsInAllChildren<T>());
            }
        }
        return componentList;
    }
}
