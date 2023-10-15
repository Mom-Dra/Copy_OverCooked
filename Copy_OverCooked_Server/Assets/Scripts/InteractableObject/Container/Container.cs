using System.Collections.Generic;
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
            if(getObject.TryFind<Tray>(out Tray tray))
            {
                return tray.ContainObjects;
            }
            return containObjects;
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

    protected bool IsFull()
    {
        return containObjects.Count == maxContainCount;
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
        if (getObject != null)
        {
            getObject = null;
            containObjects.Clear();
            if (uIComponent.HasImage)
            {
                uIComponent.Clear();
            }
        }
        //if (getObject == interactableObject)
        //{
        //    getObject = null;
        //    containObjects.Clear();
        //    if(uIComponent.HasImage)
        //    {
        //        uIComponent.Clear();
        //    }
        //} else if (getObject != null && getObject.TryFind<Container>(out Container container))
        //{
        //    container.Remove(interactableObject);
        //}
    }

    public virtual bool TryPut(SendContainerArgs sendContainerArgs)
    {
        if (getObject != null && getObject.TryFind<Container>(out Container container))
        {
            return container.TryPut(sendContainerArgs);
        }
        if (!IsFull() && IsValidObject(sendContainerArgs.Item))
        {
            sendContainerArgs.OnReceive();
            Put(sendContainerArgs.Item);
            return true;
        }
        return false;
    }

    public virtual void Put(InteractableObject interactableObject)
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

    public override EObjectType GetTopType()
    {
        if (getObject != null)
        {
            return getObject.GetTopType();
        }
        return base.GetTopType();
    }

    public Container GetTopContainer()
    {
        if (getObject != null && getObject.TryFind<Tray>(out Tray getTray))
        {
            return getTray;
        } else
        {
            return this;
        }
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