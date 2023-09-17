using System.Collections.Generic;
using UnityEngine;

public abstract class Container : InteractableObject
{
    [SerializeField]
    protected int maxContainCount = 1; // �ִ� ���� ����
    [SerializeField]
    protected Vector3 offset = new Vector3(0, 1f, 0); // ��ü ������ 
    [SerializeField]
    protected bool IsGrabbable = false;

    protected List<InteractableObject> containObjects = new List<InteractableObject>();

    [SerializeField]
    protected InteractableObject getObject;

    protected bool IsEmpty()
    {
        return containObjects.Count == 0;
    }

    protected bool IsFull()
    {
        return containObjects.Count == maxContainCount;
    }


    public virtual InteractableObject Get()
    {
        if (IsGrabbable)
        {
            return this;
        }
        else
        {
            InteractableObject io = getObject;
            getObject = null;
            containObjects.Clear();
            io?.Free();
            return io;
        }
    }

    // List ù��° �������� 
    // getObject ��������  

    public virtual bool Put(InteractableObject interactableObject)
    {
        if(!IsFull())
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

    public bool CanPut(InteractableObject interactableObject)
    {
        return !IsFull() && IsValidObject(interactableObject);
    }

    public abstract void Fit(InteractableObject gameObject);
    public abstract bool IsValidObject(InteractableObject gameObject);
}