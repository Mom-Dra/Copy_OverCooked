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
    protected Vector2 uIOffset = new Vector2 (0f, 75f);

    protected UIComponent uIComponent = new UIComponent();
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

    public UIComponent UIComponent 
    {
        get 
        { 
            return uIComponent; 
        }
        set
        {
            uIComponent = value;
        }
    }

    public virtual Image Image
    {
        set
        {
            if(value != null)
            {
                uIComponent.Add(value);
                uIComponent.OnUIPositionChanging(transform, uIOffset);
            } 
            else
            {
                uIComponent.DestroyAllImages();
            }
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