using System.Collections.Generic;
using UnityEngine;

public abstract class Container : InteractableObject
{
    [Header("Container")]
    [SerializeField]
    protected int maxContainCount = 1; // �ִ� ���� ����
    [SerializeField]
    protected Vector3 containOffset = Vector3.up; // ��ü ������ 
    
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

    // Container(this) ��ü�� ��ȯ����, GetObject�� ��ȯ����
    // �̰��� Get() �Լ��� �������� �ʰ� EmptyHandState-GrabAndPut() ���� �Ǻ��ϴ� ����
    // : �տ� Container�� ��� ���� ��, Get() �Լ��� ȣ���ϸ� this�� ��ȯ�ǰ� �ȴ� -> �̷��� �ȵ�
    // ���� RemoveGetObject() �Լ��� �����, Get() �Լ��� �����ص� �ȴ�. 
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

    // List ù��° �������� 
    // getObject ��������  

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