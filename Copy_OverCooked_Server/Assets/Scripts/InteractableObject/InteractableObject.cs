using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Interactable Object")]
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected EObjectType eObjectType;

    [HideInInspector]
    public bool IsInteractable = true;

    public virtual EObjectType GetShownType()
    {
        return eObjectType;
    }

    public virtual bool TryGet<T> (out T result) 
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