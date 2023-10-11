using System.Collections.Generic;
using UnityEngine;

public class Container : InteractableObject
{
    [Header("Container")]
    [SerializeField]
    protected int maxContainCount = 1;
    [SerializeField]
    protected bool flammablity = true; // 가연성 
    [SerializeField]
    protected Vector3 displayOffset = Vector3.up;

    [Header("Debug")]
    [SerializeField]
    protected InteractableObject getObject;
    [SerializeField]
    protected List<InteractableObject> containObjects = new List<InteractableObject>();

    // Property
    public bool Flammablity 
    { 
        get => flammablity; 
    }

    public InteractableObject GetObject 
    { 
        get => getObject; 
    }

    public List<InteractableObject> ContainObjects 
    {
        get
        {
            if(getObject.TryGet<Tray>(out Tray tray))
            {
                return tray.ContainObjects;
            }
            return containObjects;
        }
    }

    private void Awake()
    {
        if (getObject != null)
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
        if (getObject == interactableObject)
        {
            getObject = null;
            containObjects.Clear();
        } else if (getObject != null && getObject.TryGet<Container>(out Container container))
        {
            container.Remove(interactableObject);
        }
    }

    public virtual bool TryPut(InteractableObject interactableObject)
    {
        if (getObject != null && getObject.TryGet<Container>(out Container container))
        {
            return container.TryPut(interactableObject);
        }
        if (!IsFull() && IsValidObject(interactableObject))
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
        interactableObject.Selectable = false;
        if (containObjects.Count == 0)
        {
            getObject = interactableObject;
        }
        containObjects.Add(interactableObject);
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