using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EHandState
{
    Empty,
    Food,
    Cookware,
    Plate,
}

public class Hand : MonoBehaviour
{
    // Hand�� �� ������ �ִ���, food����, cookware����
    
    // ���� �տ� � Interactable�� Trigger�� �Ǿ� �ִ���, Cookware�� �ִ���??, 

    private HandState handstate;

    private InteractableBox interactableBox;
    public GameObject CurrentObject;

    public GameObject TriggeredGrabbableObject { get =>interactableBox.GetClosestGrabbable(); }
    public GameObject TriggeredInteractableObject { get => interactableBox.GetClosestInteractable(); }

    private void Awake()
    {
        handstate = EmptyHandState.Instance;

        interactableBox = GetComponent<InteractableBox>();
    }

    public void ChangeState(HandState nextState)
    {
        handstate = nextState;
    }

    public void GrabAndPut()
    {
        handstate.GrabAndPut(this);
    }

    public void InteractAndThorw()
    {
        handstate.InteractAndThorw(this);
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
        GameObject triggeredGrabbableObject = hand.TriggeredGrabbableObject;
        if (triggeredGrabbableObject != null)
        {
            EHandState? eHandState = triggeredGrabbableObject.GetEHandState();
            if (eHandState != null)
            {
                if (eHandState == EHandState.Cookware)
                {
                    // ���� ������ �ϼ��� ������ ������
                    // 1. ������ ������ 
                    // 2. ���� ���� ��ü�� ��� 
                    //GameObject getObject = triggeredGrabbableObject.GetComponent<Cookware>().GetObject();
                    //hand.CurrentObject = getObject;
                }
                else
                {
                    hand.CurrentObject = triggeredGrabbableObject;
                }

                hand.ChangeState(triggeredGrabbableObject.GetHandState());
            }
        }
    }

    public override void InteractAndThorw(Hand hand)
    {
        EHandState? eHandState = hand.TriggeredInteractableObject.GetEHandState();
        if (eHandState != null)
        {
            if (eHandState == EHandState.Cookware)
            {
                // �����ϱ� ���
                //hand.TriggeredInteractableObject.GetComponent<Cookware>().Interact();
            }
        }
    }
}

public class FoodHandState : HandState
{
    public static readonly FoodHandState Instance = new FoodHandState();

    private FoodHandState() { }

    public override void GrabAndPut(Hand hand)
    {
        GameObject triggeredGrabbableObject = hand.TriggeredGrabbableObject;
        
        if(triggeredGrabbableObject != null)
        {
            EHandState? eHandState = triggeredGrabbableObject.GetEHandState();
            if (eHandState != null)
            {
                switch (eHandState)
                {   // Empty
                    // Food
                    // Cookware
                    // Plate
                    case EHandState.Food:
                        // �ٴڿ� ���� ����
                        break;

                    case EHandState.Cookware:
                        // ���� �ű��
                        break;

                    case EHandState.Plate:
                        // ���� �ű��
                        //GameObject getObject = hand.CurrentObject.GetComponent<Cookware>().GetObject();
                        //hand.TriggeredGrabbableObject.GetComponent<Plate>().PutObject(getObject);
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
        // Put
        
    }

    public override void InteractAndThorw(Hand hand)
    {
        // ������ �ڵ�
    }
}

public class CookwareHandState : HandState
{
    public static readonly CookwareHandState Instance = new CookwareHandState();

    private CookwareHandState() { }

    public override void GrabAndPut(Hand hand)
    {
        GameObject triggeredGrabbableObject = hand.TriggeredGrabbableObject;
        if(triggeredGrabbableObject != null)
        {
            EHandState? eHandState = hand.TriggeredGrabbableObject.GetEHandState();
            if (eHandState != null)
            {
                switch (eHandState)
                {   // Empty
                    // Food
                    // Cookware
                    // Plate
                    case EHandState.Food:
                        // ���� �ű��
                        break;

                    case EHandState.Cookware:
                        // ���� �ű��
                        break;

                    case EHandState.Plate:
                        // ���� �ű��
                        //GameObject getObject = hand.CurrentObject.GetComponent<Cookware>().GetObject();
                        //hand.TriggeredGrabbableObject.GetComponent<Plate>().PutObject(getObject);
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
    }

    public override void InteractAndThorw(Hand hand)
    {
        
    }
}

public class PlateHandState : HandState
{
    public static readonly PlateHandState Instance = new PlateHandState();

    private PlateHandState() { }

    public override void GrabAndPut(Hand hand)
    {
        GameObject triggeredGrabbableObject = hand.TriggeredGrabbableObject;

        if(triggeredGrabbableObject != null)
        {
            EHandState? eHandState = triggeredGrabbableObject.GetEHandState();
            if (eHandState != null)
            {
                switch (eHandState)
                {   // Empty
                    // Food
                    // Cookware
                    // Plate
                    case EHandState.Food:
                        // ���ÿ� ���� ���
                        break;

                    case EHandState.Cookware:
                        // ���� �ű��
                        //GameObject getObject = hand.CurrentObject.GetComponent<Plate>().GetObject();
                        //hand.TriggeredGrabbableObject.GetComponent<Cookware>().PutObject(getObject);
                        break;

                    case EHandState.Plate:
                        // ���� �ű��
                        //GameObject getObject = hand.CurrentObject.GetComponent<Cookware>().GetObject();
                        //hand.TriggeredGrabbableObject.GetComponent<Plate>().PutObject(getObject);
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
        }
    }

    public override void InteractAndThorw(Hand hand)
    {

    }
}
