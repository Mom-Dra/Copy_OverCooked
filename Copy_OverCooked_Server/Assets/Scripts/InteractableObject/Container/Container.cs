using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : InteractableObject
{
    [Header("Container")]
    [SerializeField]
    protected int maxContainCount = 1;
    [SerializeField]
    protected bool flammablity = true; // °¡¿¬¼º 
    [SerializeField]
    protected Vector3 displayOffset = Vector3.up;

    [Header("Debug")]
    [SerializeField]
    protected InteractableObject getObject;

    // Property
    public bool Flammablity 
    { 
        get => flammablity; 
    }

    public InteractableObject GetObject 
    { 
        get => getObject;
        set
        {
            getObject = value;
            Fit(getObject);
        }
    }

    public Container TopContainer
    {
        get
        {
            if (getObject != null && getObject.TryFind<Tray>(out Tray getTray))
            {
                return getTray;
            } 
            else
            {
                return this;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (getObject != null)
        {
            Put(getObject);
        }
    }

    public bool HasObject()
    {
        return getObject != null;
    }

    public override bool TryFind<T>(out T result)
    {
        result = default(T);
        if (base.TryFind<T>(out T value))
        {
            result = value;
        } else if (getObject != null)
        {
            if (getObject.TryFind<T>(out T value2))
            {
                result = value2;
            }
        }
        return result != null;
    }

    public virtual bool TryGet(out InteractableObject result)
    {
        result = null;
        if (CanGet())
        {

            result = getObject;
        }
        return result != null;
    }

    protected virtual bool CanGet()
    {
        return true;
    }
     
    public virtual void Remove()
    {
        getObject = null;
    }

    public virtual bool TryPut(InteractableObject interactableObject)
    {
        if (getObject != null && getObject.TryFind<Container>(out Container container))
        {
            return container.TryPut(interactableObject);
        }
        if (IsValidObject(interactableObject))
        {
            Put(interactableObject);
            return true;
        }
        return false;
    }

    public virtual void Put(InteractableObject interactableObject)
    {
        gameObject.DebugName($"Put -> {interactableObject.name}", EDebugColor.Orange);
        getObject = interactableObject;
        Fit(getObject);
    }

    public override EObjectType GetTopType()
    {
        if (getObject != null)
        {
            return getObject.GetTopType();
        }
        return base.GetTopType();
    }

    

    protected virtual bool IsValidObject(InteractableObject serialObjects)
    {
        return getObject == null;
    }

    protected virtual void Fit(InteractableObject interactableObject)
    {
        interactableObject.Selectable = false;
        interactableObject.transform.position = transform.position + displayOffset;
        interactableObject.Fix();
    }
}