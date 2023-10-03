using UnityEditor;
using UnityEditor.Purchasing;
using UnityEngine;

public class Container : InteractableObject
{
    [Header("Container")]
    [SerializeField]
    protected Vector3 containOffset = Vector3.up;

    public InteractableObject getObject;

    private void Awake()
    {
        if(getObject != null)
        {
            Put(getObject);
        }
    }

    public bool HasObject()
    {
        return getObject != null;
    }

    public override bool TryGet<T>(out T result) 
    {
        result = default(T);
        if (base.TryGet<T>(out T value))
        {
            if (CanGet())
            {
                gameObject.DebugName("<- TryGet", EDebugColor.Red);
                result = value;
            }
        }
        else if (getObject != null)
        {
            getObject.TryGet<T>(out result);
        }
        return result != null;
    }

    protected virtual bool CanGet()
    {
        return true;
    }

    public void Remove(InteractableObject interactableObject)
    {
        // 1. 타입으로 없애기
        // 2. 객체 일치로 없애기  
        if(getObject == interactableObject)
        {
            getObject = null;
        }else if (getObject != null && getObject.TryGet<Container>(out Container container))
        {
            container.Remove(interactableObject);
        }
    }

    public bool TryPut(InteractableObject interactableObject)
    {
        if(getObject != null && getObject.TryGet<Container>(out Container container))
        {
            return container.TryPut(interactableObject);
        }
        if(IsValidObject(interactableObject))
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
        getObject = interactableObject;
    }

    public InteractableObject Get()
    {
        return getObject;
    }

    public override EObjectType GetShownType()
    {
        if(getObject != null)
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
        interactableObject.transform.position = transform.position + containOffset;
        interactableObject.Fix();
    }
}