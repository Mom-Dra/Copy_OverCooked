using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : SerializedObject
{
    public static readonly float BRIGTNESS = 0.3f;

    [Header("Interactable Object")]
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected EObjectType eObjectType;
    [SerializeField]
    protected Vector2 uIOffset = new Vector2 (0f, 75f);

    protected bool selectable = true;
    protected bool onGlowShader = false;
            
    // Property
    public bool Selectable 
    {      
        get => selectable; 
        set 
        {  
            selectable = value; 
        }  
    }

    public Vector3 UIOffset 
    {
        get => uIOffset; 
    }

    public EObjectType ObjectType
    {
        get => eObjectType;
    }

    public virtual EObjectType GetTopType()
    {
        return eObjectType;
    }

    public virtual bool TryFind<T>(out T result)
    {
        return TryGetComponent<T>(out result);
    }

    public virtual void GlowOn()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_Brightness", BRIGTNESS);
        onGlowShader = true;
    }

    public virtual void GlowOff()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_Brightness", 0f);
        onGlowShader = false;
    }

    public override bool Equals(object other)
    {
        InteractableObject io = other as InteractableObject;
        return Name.Equals(io.Name) && eObjectType == io.eObjectType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), name);
    }
}