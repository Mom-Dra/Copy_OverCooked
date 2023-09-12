using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectType
{
    Food,
    Container
}

public class InteractableObject : MonoBehaviour
{
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
}
