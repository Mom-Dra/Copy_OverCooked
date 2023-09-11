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
    // Hand�� �� ������ �ִ���, food����, cookware����
    
    // ���� �տ� � Interactable�� Trigger�� �Ǿ� �ִ���, Cookware�� �ִ���??, 

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
            if (eHandState == EHoldState.Container) // ���� �ʿ� 
            {
                // �����ϱ� ���
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
                        // ���� �ű��
                        hand.CurrentObject.GetComponent<Containable>().Put(triggeredGrabbableObject);
                        break;
                    default:
                        hand.CurrentObject = null;
                        break;
                }
            }
            else
            {
                // ���� ����
            }
        }
        else
        {
            // ���� �ٴڿ� ����
            hand.CurrentObject = null;
        }
        hand.ChangeState(triggeredGrabbableObject.GetHandState());
    }

    public override void InteractAndThorw(Hand hand)
    {
        // ������ �ڵ�
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
                        // ���� �ű��
                        hand.CurrentObject.GetComponent<Containable>().Put(triggeredGrabbableObject);
                        break;
                    default:
                        // �ٴڿ� ���� 
                        hand.CurrentObject = null;
                        break;
                }
            }
            else
            {
                // �ٴڿ� ����
                hand.CurrentObject = null;
            }
        }
        else
        {
            // �ٴڿ� ����
            hand.CurrentObject = null;
        }
        hand.ChangeState(triggeredGrabbableObject.GetHandState());
    }

    public override void InteractAndThorw(Hand hand)
    {
        
    }
}
