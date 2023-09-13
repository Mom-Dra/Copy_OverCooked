using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

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
    [SerializeField]
    private Interactor interactor;
    [HideInInspector]
    public InteractableObject CurrentObject;

    public InteractableObject TriggeredObject { get =>interactor.GetTriggeredObject(); }

    private void Awake()
    {
        handState = EmptyHandState.Instance;

        //interactor = GetComponent<Interactor>();
    }

    private void Update()
    {
        if(CurrentObject != null)
        {
            CurrentObject.transform.position = transform.position;
        }
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
        Debug.Log("Change State : " +  handState);
    }

    public void GrabAndPut()
    {
        handState.GrabAndPut(this);
    }

    public void InteractAndThorw()
    {
        handState.InteractAndThorw(this);
    }

    public void PutAway()
    {
        if(CurrentObject != null)
        {
            CurrentObject.IsInteractable = true;
            CurrentObject = null;
        }
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
        // 탐지된 오브젝트 가져오기 
        InteractableObject triggeredObject = hand.TriggeredObject;
        // 탐지된 오브젝트가 있다면,
        if (triggeredObject != null)
        {
            // 탐지된 오브젝트의 ObjectType 검사 (이거 tag로 해도 되겠네 // ? )
            EObjectType objectType = triggeredObject.GetObjectType();
            // ObjectType = Container 라면,
            if (objectType == EObjectType.Container)
            {
                // Container에서 물체를 가져와서 hand에 넣음 
                GameObject getObject = triggeredObject.GetComponent<Containable>().Get();
                hand.CurrentObject = getObject.GetComponent<InteractableObject>();
                hand.CurrentObject.IsInteractable = false;
            } else
            {
                hand.CurrentObject = triggeredObject;
                hand.CurrentObject.IsInteractable = false;
            }

            hand.UpdateState();
        }
    }
    public override void InteractAndThorw(Hand hand)
    {
        // 탐지된 오브젝트 가져오기
        InteractableObject triggeredObject = hand.TriggeredObject;
        // 오브젝트가 <Cookware> 스크립트를 가지고 있는지 확인
        
        // 가지고 있다면,
        if(triggeredObject.TryGetComponent<Cookware>(out Cookware cookware))
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
                    hand.PutAway();
                }
            } else
            {
                hand.PutAway();
            }
        }
        else
        {
            // 음식 바닥에 놓기
            hand.PutAway();
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
            // 조리도구 -> 접시 = 음식이 옮겨짐
            // 조리도구 -> 선반 = 조리도구가 옮겨짐 
            // 케이스 수정 바람 
            if (objectType == EObjectType.Container)
            {
                GameObject getObject = hand.CurrentObject.GetComponent<Containable>().Get();
                if (getObject != null && getObject.tag == "Food")
                {
                    if (triggeredObject.GetComponent<Containable>().Put(getObject))
                    {
                        hand.PutAway();
                    }
                }
            } else
            {
                hand.PutAway();
            }
        }
        else
        {
            // 바닥에 놓기
            hand.PutAway();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        
    }
}
