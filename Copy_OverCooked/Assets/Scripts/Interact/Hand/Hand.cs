using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public enum EHoldState
{
    Empty,
    Food,
    Container,
}

public class Hand : MonoBehaviour
{
    // Hand에 뭘 가지고 있는지, food인지, cookware인지
    
    // 현재 앞에 어떤 Interactable이 Trigger이 되어 있는지, Cookware가 있는지??, 

    private HoldState holdState;

    private InteractableBox interactableBox;
    public GameObject CurrentObject;

    public GameObject TriggeredGrabbableObject { get =>interactableBox.GetClosestGrabbable(); }
    public GameObject TriggeredInteractableObject { get => interactableBox.GetClosestInteractable(); }

    private void Awake()
    {
        holdState = EmptyHandState.Instance;

        interactableBox = GetComponent<InteractableBox>();
    }

    public void ChangeState(HoldState nextState)
    {
        holdState = nextState;
    }

    public void GrabAndPut()
    {
        holdState.GrabAndPut(this);
    }

    public void InteractAndThorw()
    {
        holdState.InteractAndThorw(this);
    }
}

public abstract class HoldState
{
    public abstract void GrabAndPut(Hand hand);

    public abstract void InteractAndThorw(Hand hand);
}

public class EmptyHandState : HoldState
{
    public static readonly EmptyHandState Instance = new EmptyHandState();
    private EmptyHandState() { }

    public override void GrabAndPut(Hand hand)
    {
        // Grab
        GameObject triggeredGrabbableObject = hand.TriggeredGrabbableObject;
        if (triggeredGrabbableObject != null)
        {
            EHoldState? eHandState = triggeredGrabbableObject.GetEHandState();
            if (eHandState != null)
            {
                if (eHandState == EHoldState.Container)
                {
                    GameObject getObject = triggeredGrabbableObject.GetComponent<Containable>().Get();
                    hand.CurrentObject = getObject;
                } else
                {
                    hand.CurrentObject = triggeredGrabbableObject;
                }

                hand.ChangeState(triggeredGrabbableObject.GetHandState());
            }
        }
    }
    public override void InteractAndThorw(Hand hand)
    {
        EHoldState? eHandState = hand.TriggeredInteractableObject.GetEHandState();
        if (eHandState != null)
        {
            if (eHandState == EHoldState.Container) // 수정 필요 
            {
                // 조리하기 명령
                hand.TriggeredInteractableObject.GetComponent<Cookware>().Interact();
            }
        }
    }
}

public class HoldFoodState : HoldState
{
    public static readonly HoldFoodState Instance = new HoldFoodState();

    private HoldFoodState() { }

    public override void GrabAndPut(Hand hand)
    {
        GameObject triggeredGrabbableObject = hand.TriggeredGrabbableObject;
        
        if(triggeredGrabbableObject != null)
        {
            EHoldState? eHandState = triggeredGrabbableObject.GetEHandState();
            if (eHandState != null)
            {
                switch (eHandState)
                {   // Empty
                    // Food
                    // Container
                    case EHoldState.Container:
                        // 음식 옮기기
                        hand.CurrentObject.GetComponent<Containable>().Put(triggeredGrabbableObject);
                        break;
                    default:
                        hand.CurrentObject = null;
                        break;
                }
            }
            else
            {
                // 음식 놓기
            }
        }
        else
        {
            // 음식 바닥에 놓기
            hand.CurrentObject = null;
        }
        hand.ChangeState(triggeredGrabbableObject.GetHandState());
    }

    public override void InteractAndThorw(Hand hand)
    {
        // 던지는 코드
    }
}

public class HoldContainerState : HoldState
{
    public static readonly HoldContainerState Instance = new HoldContainerState();

    private HoldContainerState() { }

    public override void GrabAndPut(Hand hand)
    {
        GameObject triggeredGrabbableObject = hand.TriggeredGrabbableObject;
        if(triggeredGrabbableObject != null)
        {
            EHoldState? eHandState = hand.TriggeredGrabbableObject.GetEHandState();
            if (eHandState != null)
            {
                switch (eHandState)
                {   // Empty
                    // Food
                    // Cookware
                    // 
                    case EHoldState.Container:
                        // 음식 옮기기
                        hand.CurrentObject.GetComponent<Containable>().Put(triggeredGrabbableObject);
                        break;
                    default:
                        // 바닥에 놓기 
                        hand.CurrentObject = null;
                        break;
                }
            }
            else
            {
                // 바닥에 놓기
                hand.CurrentObject = null;
            }
        }
        else
        {
            // 바닥에 놓기
            hand.CurrentObject = null;
        }
        hand.ChangeState(triggeredGrabbableObject.GetHandState());
    }

    public override void InteractAndThorw(Hand hand)
    {
        
    }
}
