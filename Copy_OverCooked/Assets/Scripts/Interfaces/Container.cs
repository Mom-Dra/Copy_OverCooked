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
    [SerializeField]
    protected InteractableObject getObject;

    protected List<InteractableObject> containObjects = new List<InteractableObject>();

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
            getObject.GetComponent<Rigidbody>().isKinematic = false;
            return getObject;
        }
    }

    // List ù��° ��������
    // getObject ��������  

    public virtual bool Put(InteractableObject gameObject)
    {
        if (!IsFull() && IsValidObject(gameObject))
        {
            if (containObjects.Count == 0)
            {
                getObject = gameObject;
            }
            containObjects.Add(gameObject);
            
            Fit(gameObject);
            return true;
        }
        return false;
    }

    public abstract void Fit(InteractableObject gameObject);
    public abstract bool IsValidObject(InteractableObject gameObject);
}