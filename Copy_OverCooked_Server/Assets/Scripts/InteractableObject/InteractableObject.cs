using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [Header("Interactable Object")]
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected EObjectType eObjectType;
    [SerializeField]
    protected Vector3 uIOffset = Vector3.up;
    
    private List<Image> uIImages = new List<Image>();
    protected bool selectable = true;

    // Property
    public bool Selectable 
    { 
        get => selectable; 
        set 
        { 
            selectable = value; 
        } 
    }

    public Image UIImage 
    {
        get
        {
            if(uIImages.Count == 0)
            {
                return null;
            }
            return uIImages[0];
        }
        set 
        {
            if (value == null)
                uIImages.Clear();
            else
                uIImages.Add(value); 
        } 
    }

    public Vector3 UIOffset 
    {
        get => uIOffset; 
    }

    public virtual EObjectType GetShownType()
    {
        return eObjectType;
    }

    public virtual bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek)
    {
        return TryGetComponent<T>(out result);
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