using System.Collections.Generic;
using UnityEngine;

public abstract class Container : InteractableObject
{
    [Header("Container")]
    [SerializeField]
    protected int maxContainCount = 1; // 최대 보관 개수
    [SerializeField]
    protected Vector3 containOffset = new Vector3(0, 1f, 0); // 물체 오프셋 
    
    public bool IsGrabbable = false;

    [SerializeField]
    protected List<InteractableObject> containObjects = new List<InteractableObject>();

    [Header("Debug")]
    [SerializeField]
    protected InteractableObject getObject;

    private void Awake()
    {
        if(getObject != null)
        {
            Put(getObject);
        }   
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(getObject != null && IsGrabbable)
        {
            getObject.transform.position = transform.position + containOffset;
        }
    }

    private bool Put(InteractableObject interactableObject)
    {
        if (!IsFull())
        {
            Fit(interactableObject);
            interactableObject.IsInteractable = false;
            if (IsEmpty())
            {
                getObject = interactableObject;
            }
            containObjects.Add(interactableObject);
            return true;
        }
        return false;
    }


    public InteractableObject GetObject()
    {
        return getObject;
    }

    protected bool IsEmpty()
    {
        return containObjects.Count == 0;
    }

    protected bool IsFull()
    {
        return containObjects.Count == maxContainCount;
    }

    public InteractableObject PeekGetObject()
    {
        if(getObject != null && getObject.TryGetComponent<Container>(out Container getContainer))
        {
            return getContainer.PeekGetObject();
        }
        return getObject;
    }

    public virtual InteractableObject Get()
    {
        InteractableObject io = getObject;
        getObject = null;
        containObjects.Clear();
        io?.Free();
        return io;
    }

    // List 첫번째 가져오기 
    // getObject 가져오기  

    public virtual bool TryPut(InteractableObject interactableObject)
    {
        gameObject.DebugName("Put", EDebugColor.Red);
        if (getObject != null && getObject.TryGetComponent<Container>(out Container getContainer))
        {
            return getContainer.TryPut(interactableObject);
        }
        
        return Put(interactableObject);
    }

    public bool CanPut(InteractableObject interactableObject)
    {
        if(getObject != null && getObject.TryGetComponent<Container>(out Container getContainer))
        {
            return getContainer.CanPut(interactableObject);
        }
        return !IsFull() && IsValidObject(interactableObject);
    }

    public override void GlowOn()
    {
        if (getObject != null)
        {
            base.GlowOff();
            getObject.GlowOn();
        }
        else
        {
            base.GlowOn(); 
        }
    }

    public override void GlowOff() 
    {
        if (getObject != null)
        {
            getObject.GlowOff();
        }
        else
        {
            base.GlowOff();
        }
    }

    public abstract void Fit(InteractableObject gameObject);
    public abstract bool IsValidObject(InteractableObject gameObject);
}