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
    [SerializeField]
    protected EObjectSerialCode objectSerialCode;
    [SerializeField]
    protected Vector3 uIOffset = Vector3.up;
    
    protected bool selectable = true;
    private Image uIImage;

    // Property
    public bool Selectable { get => selectable; set { selectable = value; } }
    public Image UIImage { get => uIImage; set { uIImage = value; } }
    public Vector3 UIOffset { get => uIOffset; }
    public EObjectSerialCode ObjectSerialCode { get => objectSerialCode; }

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