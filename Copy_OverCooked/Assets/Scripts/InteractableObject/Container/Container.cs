using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;

public class Container : InteractableObject
{
    [Header("Container")]
    [SerializeField]
    protected int maxContainCount = 1;
    public bool IsFirable = true;
    [SerializeField]
    protected Vector3 displayOffset = Vector3.up;

    [Header("Debug")] 
    public InteractableObject getObject;
    [SerializeField]
    public List<InteractableObject> containObjects = new List<InteractableObject>();

    private void Awake()
    {
        if(getObject != null)
        {
            Put(getObject);
        }
    }

    protected bool IsFull()
    {
        return containObjects.Count == maxContainCount;
    }

    public bool HasObject()
    {
        return getObject != null;
    }

    public override bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek)
    {
        result = default(T);
        if (base.TryGet<T>(out T value))
        {
            result = value;
        } else if (getObject != null)
        {
            if (getObject.TryGet<T>(out T value2) && (getMode == EGetMode.Peek || CanGet()))
            {
                result = value2;
            }
        }
        return result != null;
    }

    protected virtual bool CanGet()
    {
        return true;
    }

    public virtual void Remove(InteractableObject interactableObject)
    {
        // 1. 타입으로 없애기
        // 2. 객체 일치로 없애기  (&&^^당첨^^&&)
        if(getObject == interactableObject)
        {
            getObject = null;
            containObjects.Clear();
        }else if (getObject != null && getObject.TryGet<Container>(out Container container))
        {
            container.Remove(interactableObject);
        }
    }

    public virtual bool TryPut(InteractableObject interactableObject)
    {
        if(getObject != null && getObject.TryGet<Container>(out Container container))
        {
            return container.TryPut(interactableObject);
        }
        if(!IsFull() && IsValidObject(interactableObject))
        {
            Put(interactableObject);
            return true;
        }
        return false;
    }

    protected virtual void Put(InteractableObject interactableObject)
    {
        gameObject.DebugName($"Put -> {interactableObject.name}", EDebugColor.Orange);
        Fit(interactableObject);
        interactableObject.IsInteractable = false;
        if(containObjects.Count == 0)
        {
            getObject = interactableObject;
        }
        containObjects.Add(interactableObject);
    }

    public InteractableObject Get()
    {
        return getObject;
    }

    public override EObjectType GetShownType()
    {
        Debug.Log("GetShownType");
        if (getObject != null)
        {
            return getObject.GetShownType();
        }
        return base.GetShownType();
    }

    protected virtual bool IsValidObject(InteractableObject interactableObject)
    {
        return getObject == null;
    }

    protected virtual void Fit(InteractableObject interactableObject)
    {
        interactableObject.transform.position = transform.position + displayOffset;
        interactableObject.Fix();
    }
}