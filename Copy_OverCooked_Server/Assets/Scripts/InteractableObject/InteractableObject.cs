using System;
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

    public float uIYOffset = 1f;

    public Image uIImage;

    [HideInInspector]
    public bool IsInteractable = true;

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