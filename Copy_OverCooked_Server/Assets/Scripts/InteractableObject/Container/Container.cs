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
    [SerializeField]
    protected List<EObjectSerialCode> containObjects = new List<EObjectSerialCode>();

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

    public List<EObjectSerialCode> ContainObjects 
    {
        get
        {
            if(getObject != null && getObject.TryFind<Tray>(out Tray tray) && tray.HasObject())
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
            Fit(getObject);
            containObjects.Add(getObject.SerialCode);
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
    }

    public virtual bool TryPut(SendObjectArgs sendContainerArgs)
    {
        if (getObject != null && getObject.TryFind<Container>(out Container container))
        {
            return container.TryPut(sendContainerArgs);
        }
        if (!IsFull() && IsValidObject(sendContainerArgs.ContainObjects.Concat(containObjects).ToList()))
        {
            Put(sendContainerArgs);
            return true;
        }
        return false;
    }

    public virtual void Put(SendObjectArgs sendContainerArgs)
    {
        gameObject.DebugName($"Put -> {sendContainerArgs.Item.name}", EDebugColor.Orange);
        getObject = sendContainerArgs.Item;
        Fit(getObject);
        
        if(sendContainerArgs.ContainObjects.Count  > 0)
        {
            containObjects.AddRange(sendContainerArgs.ContainObjects);
        }
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

    protected virtual bool IsValidObject(List<EObjectSerialCode> serialObjects)
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