using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : SerializedObject
{
    public static readonly float BRIGTNESS = 0.3f;

    [Header("Interactable Object")]
    public string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected EObjectType objectType;
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
        get => objectType;
    }

    [ContextMenu("Initialize")]
    private void SetSerialCode()
    {
        Name = name;
        if (Enum.TryParse(Name, out EObjectSerialCode sc))
        {
            serialCode = sc;
            int scToInt = (int)serialCode;
            if((Name.Equals("Chopped_Dough") || Name.Equals("Tortilla")) || (scToInt > 0 && scToInt < 20))
            {
                objectType = EObjectType.Tray;
            }else if(scToInt >= 20 && scToInt < 30)
            {
                objectType = EObjectType.Other;
            }else if(scToInt >= 30 && scToInt < 100)
            {
                objectType = EObjectType.Empty_Fixed_Container;
            }else  if(scToInt >= 100 && scToInt < 400)
            {
                objectType = EObjectType.Food;
            }
        }
    }

    public virtual EObjectType GetTopType()
    {
        return objectType;
    }


    public virtual bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek)
    {
        return TryGetComponent<T>(out result);
    }

    public virtual void GlowOn()
    {
        Renderer renderer = GetComponent<Renderer>();
        //renderer?.material?.SetFloat("_Brightness", BRIGTNESS);
        onGlowShader = true;
    }

    public virtual void GlowOff()
    {
        Renderer renderer = GetComponent<Renderer>();
        //renderer?.material?.SetFloat("_Brightness", 0f);
        onGlowShader = false;
    }

    public override bool Equals(object other)
    {
        InteractableObject io = other as InteractableObject;
        return Name.Equals(io.Name) && objectType == io.objectType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), name);
    }
}