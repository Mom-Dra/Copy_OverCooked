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
    // Hand에 뭘 가지고 있는지, food인지, cookware인지
    
    // 현재 앞에 어떤 Interactable이 Trigger이 되어 있는지, Cookware가 있는지??, 

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
                    // 조리 도구에 완성된 음식이 있으면
                    // 1. 음식을 꺼낸다 
                    // 2. 조리 도구 자체를 든다 
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
                // 조리하기 명령
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
                        // 바닥에 내려 놓기
                        break;

                    case EHandState.Cookware:
                        // 음식 옮기기
                        break;

                    case EHandState.Plate:
                        // 음식 옮기기
                        //GameObject getObject = hand.CurrentObject.GetComponent<Cookware>().GetObject();
                        //hand.TriggeredGrabbableObject.GetComponent<Plate>().PutObject(getObject);
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
        // Put
        
    }

    public override void InteractAndThorw(Hand hand)
    {
        // 던지는 코드
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
                        // 음식 옮기기
                        break;

                    case EHandState.Cookware:
                        // 음식 옮기기
                        break;

                    case EHandState.Plate:
                        // 음식 옮기기
                        //GameObject getObject = hand.CurrentObject.GetComponent<Cookware>().GetObject();
                        //hand.TriggeredGrabbableObject.GetComponent<Plate>().PutObject(getObject);
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
                        // 접시에 음식 담기
                        break;

                    case EHandState.Cookware:
                        // 음식 옮기기
                        //GameObject getObject = hand.CurrentObject.GetComponent<Plate>().GetObject();
                        //hand.TriggeredGrabbableObject.GetComponent<Cookware>().PutObject(getObject);
                        break;

                    case EHandState.Plate:
                        // 음식 옮기기
                        //GameObject getObject = hand.CurrentObject.GetComponent<Cookware>().GetObject();
                        //hand.TriggeredGrabbableObject.GetComponent<Plate>().PutObject(getObject);
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
        }
    }

    public override void InteractAndThorw(Hand hand)
    {

    }
}
