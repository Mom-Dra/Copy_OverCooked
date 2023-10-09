using System;
using UnityEngine;

public enum EObjectType
{
    Food,
    Container
}

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    protected int Id;
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected EObjectType objectType;

    [HideInInspector]
    public bool IsInteractable = true;

    public EObjectType GetObjectType()
    {
        return objectType;
    }

    public override bool Equals(object other)
    {
        InteractableObject io = other as InteractableObject;
        return Id == io.Id && Name == io.Name;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(Name);
        hash.Add(Id);
        hash.Add(objectType);
        return hash.ToHashCode();
    }
}
