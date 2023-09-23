using System.Collections.Generic;
using UnityEngine;

public abstract class Container : InteractableObject
{
    [Header("Container")]
    [SerializeField]
    protected int maxContainCount = 1; // 최대 보관 개수
    [SerializeField]
    protected Vector3 containOffset = Vector3.up; // 물체 오프셋 
    
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
            getObject.Fix();
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

    protected virtual bool Put(InteractableObject interactableObject)
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

    protected virtual bool ThrowPut(InteractableObject interactableObject)
    {
        if (Put(interactableObject))
        {
            interactableObject.Fix();
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

    public InteractableObject Peek()
    {
        return getObject;
    }

    public InteractableObject PeekGetObject()
    {
        if(getObject != null && getObject.TryGetComponent<Container>(out Container getContainer))
        {
            return getContainer.PeekGetObject();
        }
        return getObject;
    }

    // Container(this) 자체를 반환할지, GetObject를 반환할지
    // 이것을 Get() 함수에 위임하지 않고 EmptyHandState-GrabAndPut() 에서 판별하는 이유
    // : 손에 Container를 들고 있을 때, Get() 함수를 호출하면 this가 반환되게 된다 -> 이러면 안됨
    // 만약 RemoveGetObject() 함수를 만들면, Get() 함수에 위임해도 된다. 
    public virtual InteractableObject Get() // pop
    {
        if(uIImage != null)
        {
            Destroy(uIImage.gameObject);
            uIImage = null;
        }
        InteractableObject io = getObject;
        getObject = null;
        containObjects.Clear();
        //io?.Free();
        io.GlowOff();
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

    public virtual bool TryThrowPut(InteractableObject interactableObject)
    {
        gameObject.DebugName("Put", EDebugColor.Red);
        if (getObject != null && getObject.TryGetComponent<Container>(out Container getContainer))
        {
            return getContainer.TryPut(interactableObject);
        }
        
        return ThrowPut(interactableObject);
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
        //else
        //{
            base.GlowOff();
        //}
    }

    public void Fit(InteractableObject interactableObject)
    {
        interactableObject.transform.position = transform.position + containOffset;
    }

    public abstract bool IsValidObject(InteractableObject interactableObject);
}