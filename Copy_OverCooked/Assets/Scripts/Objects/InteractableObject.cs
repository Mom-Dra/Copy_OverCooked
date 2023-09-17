using System;
using UnityEngine;
using UnityEngine.UI;

public enum EObjectType
{
    Food,
    Container
}

public class InteractableObject : MonoBehaviour
{
    [Header("Interactable Object")]
    [SerializeField]
    protected int Id;
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected EObjectType objectType;

    [Header("UI")]
    public Vector3 uIOffset = new Vector3(0, 80f, 0);
    [HideInInspector]
    public Image uIImage = null;

    [HideInInspector]
    public bool IsInteractable = true;

    protected virtual void FixedUpdate()
    {
        if(uIImage != null)
        {
            uIImage.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + uIOffset;
        }
    }


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
