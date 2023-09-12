using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

// 플레이어의 손에 있는 오브젝트의 상태 
public enum EHandState
{
    Empty,
    Food,
    Container,
}

public class Hand : MonoBehaviour
{
    private HandState handState;

    private Interactor interactor;
    [HideInInspector]
    public InteractableObject CurrentObject;

    public InteractableObject TriggeredObject { get =>interactor.GetTriggeredObject(); }

    private void Awake()
    {
        handState = EmptyHandState.Instance;

        interactor = GetComponent<Interactor>();
    }

    public void UpdateState()
    {
        if(CurrentObject != null)
        {
            switch (CurrentObject.GetObjectType())
            {
                case EObjectType.Food:
                    handState = FoodHandState.Instance;
                    break;
                case EObjectType.Container:
                    handState = ContainerHandState.Instance;
                    break;
            }
        }
        else
        {
            handState = EmptyHandState.Instance;
        }
    }

    public void GrabAndPut()
    {
        handState.GrabAndPut(this);
    }

    public void InteractAndThorw()
    {
        handState.InteractAndThorw(this);
    }
}

public abstract class HandState
{
    public abstract void GrabAndPut(Hand hand);

    public abstract void InteractAndThorw(Hand hand);
}

public class EmptyHandState : HandState
{
    public static readonly EmptyHandState Instance = new EmptyHandState();
    private EmptyHandState() { }

    public override void GrabAndPut(Hand hand)
    {
        // Grab
        InteractableObject triggeredObject = hand.TriggeredObject;
        if (triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
                if (objectType == EObjectType.Container)
                {
                    GameObject getObject = triggeredObject.GetComponent<Containable>().Get();
                    hand.CurrentObject = getObject.GetComponent<InteractableObject>();
                } else
                {
                    hand.CurrentObject = triggeredObject;
                }

                hand.UpdateState();
        }
    }
    public override void InteractAndThorw(Hand hand)
    {
        // 아래의 코드가 심히 맘에 안듬 (추후 구조 변경 요망)

        // 탐지된 오브젝트 가져오기
        InteractableObject triggeredObject = hand.TriggeredObject;
        // 오브젝트가 <Cookware> 스크립트를 가지고 있는지 확인
        Cookware cookware = triggeredObject.GetComponent<Cookware>();
        // 가지고 있다면,
        if(cookware != null)
        {
            // 해당 조리도구와 상호작용 (요리)
            cookware.Interact();
        }
    }
}

public class FoodHandState : HandState
{
    public static readonly FoodHandState Instance = new FoodHandState();

    private FoodHandState() { }

    public override void GrabAndPut(Hand hand)
    {
        InteractableObject triggeredObject = hand.TriggeredObject;
        
        if(triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
            if (objectType == EObjectType.Container)
            {
                if (triggeredObject.GetComponent<Containable>().Put(hand.CurrentObject.gameObject))
                {
                    hand.CurrentObject = null;
                }
            } else
            {
                hand.CurrentObject = null;
            }
        }
        else
        {
            // 음식 바닥에 놓기
            hand.CurrentObject = null;
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        // 던지는 코드
        Debug.Log("Throw : " +  hand.gameObject.name);
        hand.CurrentObject = null;
    }
}

public class ContainerHandState : HandState
{
    public static readonly ContainerHandState Instance = new ContainerHandState();

    private ContainerHandState() { }

    public override void GrabAndPut(Hand hand)
    {
        InteractableObject triggeredObject = hand.TriggeredObject;
        if(triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
            if (objectType == EObjectType.Container)
            {
                GameObject getObject = hand.CurrentObject.GetComponent<Containable>().Get();
                if (getObject != null && getObject.tag == "Food")
                {
                    if (triggeredObject.GetComponent<Containable>().Put(getObject))
                    {
                        hand.CurrentObject = null;
                    }
                }
            } else
            {
                hand.CurrentObject = null;
            }
        }
        else
        {
            // 바닥에 놓기
            hand.CurrentObject = null;
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        
    }
}
