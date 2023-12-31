using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : InteractableObject
{
    [Header("Container")]
    
    
    [SerializeField]
    protected Vector3 displayOffset = Vector3.up;

    [Header("Debug")]
    [SerializeField]
    protected InteractableObject getObject;

    

    public virtual InteractableObject GetObject 
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
            if (getObject != null && getObject.TryGet<Tray>(out Tray getTray) && !getObject.GetComponent<FoodTray>())
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

    public override void GlowOn()
    {
        base.GlowOn();
        if(HasObject()) getObject.GlowOn();
    }

    public override void GlowOff()
    {
        base.GlowOff();
        if(HasObject()) getObject.GlowOff();
    }

    public virtual bool HasObject()
    {
        return getObject != null;
    }

    public override bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek) // 함수 2개로 분리
    {
        // 버전 2 : 탐색 우선순위가 가장 위에 있는 물체부터
        // (버전 1, 2 모두 상관없는듯 함 일단은, 주석 코드 지우지 말것)
        //result = default(T);
        //if (getObject != null && getObject.TryGet<T>(out T value2, getMode) && (getMode == EGetMode.Peek || CanGet()))
        //{
        //    result = value2;
        //} 
        //else if (base.TryGet<T>(out T value, getMode))
        //{ 
        //    result = value;
        //}

        //return result != null;

        // 버전 1 : 탐색 우선순위가 가장 밑에 있는 물체부터 
        result = default(T);
        if (base.TryGet<T>(out T value, getMode))
        {
            result = value;
        } else if (getObject != null)
        {
            if (getObject.TryGet<T>(out T value2, getMode) && (getMode == EGetMode.Peek || CanGet()))
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
        if (getObject == interactableObject)
        {
            getObject = null;
        } 
        else if (getObject != null && getObject.TryGetComponent<Container>(out Container container))
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
        if (IsValidObject(interactableObject))
        {
            Put(interactableObject);
            return true;
        }
        return false;
    }

    public virtual void Put(InteractableObject interactableObject)
    {
        gameObject.DebugName($"Put {Name} -> {interactableObject.name}", EDebugColor.Orange);
        GetObject = interactableObject;
        //Fit(getObject);

        if (onGlowShader)
        {
            getObject.GlowOn();
        } else
        {
            getObject.GlowOff();
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

    

    protected virtual bool IsValidObject(InteractableObject interactableObject)
    {
        return getObject == null;
    }

    protected virtual void Fit(InteractableObject interactableObject)
    {
        interactableObject.Selectable = false;
        interactableObject.transform.position = transform.position + displayOffset;
        interactableObject.transform.parent = transform;
        interactableObject.Fix();
    }

    protected virtual void ThrowPut(InteractableObject interactableObject)
    {
        TryPut(interactableObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
         if (collision.transform.TryGetComponent<Food>(out Food food) && food.Selectable)
        {
            ThrowPut(food);
        }
    }

    
}