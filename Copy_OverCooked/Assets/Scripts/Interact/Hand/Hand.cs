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
    private Player player;
    private HandState handState;
    public Interactor interactor { get; private set; }

    [Header("Throw")]
    public float throwPower;

    [HideInInspector]
    public InteractableObject CurrentObject;

    private Rigidbody currentObjectRigid;

    public InteractableObject TriggeredObject { get => interactor.ClosestInteractableObject; }

    private void Awake()
    {
        handState = EmptyHandState.Instance;

        interactor = transform.parent.GetComponentInChildren<Interactor>();
    }

    private void FixedUpdate()
    {
        if (currentObjectRigid != null)
        {
            currentObjectRigid.position = transform.position;
            currentObjectRigid.rotation = transform.rotation;
        }
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void UpdateState()
    {
        if (CurrentObject != null)
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
        } else
        {
            handState = EmptyHandState.Instance;
        }

        Debug.Log($"<color=yellow> {handState} </color>");
    }

    public void GrabAndPut()
    {
        handState.GrabAndPut(this);
    }

    public void InteractAndThorw()
    {
        handState.InteractAndThorw(this);
    }

    public void HoldIn(InteractableObject interactableObject)
    {
        if (interactableObject != null)
        {
            CurrentObject = interactableObject;
            CurrentObject.IsInteractable = false;
            currentObjectRigid = CurrentObject.GetComponent<Rigidbody>();

            CurrentObject.Fix();
            //CurrentObject.GlowOff();
        }
    }

    public void HoldOutAndFree() // Free를 할 지 말지 결정 
    {
        if (CurrentObject != null)
        {
            CurrentObject.Free();

            CurrentObject.IsInteractable = true;
            CurrentObject = null;
            currentObjectRigid = null;
        }
    }

    public void HoldOut()   
    {
        if (CurrentObject != null)
        {
            //CurrentObject.Free();

            //CurrentObject.IsInteractable = true;
            CurrentObject = null;
            currentObjectRigid = null;
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
            EObjectType objectType = triggeredObject.GetObjectType();
            // ObjectType = Container 라면,
            if (objectType == EObjectType.Container)
            {
                // Container에서 물체를 가져와서 hand에 넣음 
                Container triggerContainer = triggeredObject.GetComponent<Container>();
                if(triggerContainer != null && triggerContainer.IsGrabbable)
                {
                    hand.HoldIn(triggerContainer);
                }
                else
                {
                    InteractableObject getObject = triggerContainer.Get();
                    hand.HoldIn(getObject);
                    getObject?.gameObject.DebugName("Hand Get : ", EDebugColor.Orange);
                }
            } else
            {
                hand.HoldIn(triggeredObject);
            }

            hand.UpdateState();
        }
    }

    public override void InteractAndThorw(Hand hand)
    {
        // 탐지된 오브젝트 가져오기
        InteractableObject triggeredObject = hand.TriggeredObject;
        // 오브젝트가 <Cookware> 스크립트를 가지고 있는지 확인 
        if (triggeredObject != null)
        {
            // 가지고 있다면, 
            if (triggeredObject.TryGetComponent<Cookware>(out Cookware cookware))
            {
                // 해당 조리도구와 상호작용 (요리) 
                if (LinkManager.Instance.Connect(hand.GetPlayer(), triggeredObject))
                {
                    cookware.Interact();
                }
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
        InteractableObject triggeredObject = hand.TriggeredObject;

        if (triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
            if (objectType == EObjectType.Container)
            {
                Container container = triggeredObject.GetComponent<Container>();
                if (container.CanPut(hand.CurrentObject))
                {
                    Debug.Log("<color=red> Hand put </color>");
                    InteractableObject io = hand.CurrentObject;
                    hand.HoldOut();
                    container.TryPut(io);
                    hand.interactor.SetClosestObject();
                }
            } 
            else
            {
                // 음식 바닥에 놓기 
                hand.HoldOutAndFree();
            }
        } else
        {
            // 음식 바닥에 놓기 
            hand.HoldOutAndFree();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        // 던지는 코드
        InteractableObject io = hand.CurrentObject;
        hand.HoldOutAndFree();
        io.GetComponent<Rigidbody>().AddForce(hand.transform.forward * hand.throwPower, ForceMode.Impulse);
        hand.UpdateState();
    }
}

public class ContainerHandState : HandState
{
    public static readonly ContainerHandState Instance = new ContainerHandState();

    private ContainerHandState() { }

    public override void GrabAndPut(Hand hand)
    {
        // 손에 들고 있는 Container(Hand)
        Container handContainer = hand.CurrentObject.GetComponent<Container>();
        
        // 탐지된 물체가 Null인지 검사 
        InteractableObject triggeredObject = hand.TriggeredObject;
        if (triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
            // 탐지된 물체가 Container라면, 
            // Container(Hand) -> Container(Triggered) 또는
            // Container(Hand) <- Container(Triggered) 이런 형식이 됨 
            if (objectType == EObjectType.Container)
            {
                // 탐지된 Container(Trigger)
                Container triggerContainer = triggeredObject.GetComponent<Container>();

                // Container(Hand) 내의 물체 GetObject(HandContainer)
                InteractableObject GetObjectInHandContainer = handContainer.PeekGetObject();
                // Container(Trigger) 내의 물체 GetObjet(TriggerContainer)
                InteractableObject GetObjectInTriggerContainer = triggerContainer.PeekGetObject();

                // Container(Trigger)가 물체를 보관하고 있는지 검사
                if (GetObjectInTriggerContainer != null)
                {
                    // Container(Trigger)가 물체를 보관하고 있다면,
                    // 아래 2가지 경우의 수를 고려해야 함 

                    // 1. 넣기(Put)
                    // : Container(Trigger)에 추가로 넣기 

                    // 2. 가져오기(Get)
                    // : Container(Trigger) 내의 물체를 가져와 Container(Hand)에 넣기


                    // 1번. 넣기(Put)

                    // GetObject(HandContainer) -> Container(Trigger)
                    if (GetObjectInHandContainer != null && triggerContainer.CanPut(GetObjectInHandContainer))
                    {
                        handContainer.Get();
                        triggerContainer.TryPut(GetObjectInHandContainer);
                    }
                    else
                    {
                        // 2번. 가져오기(Get) 

                        // Container(Trigger) 내의 물체를 꺼내서 Container(Hand)로 가져오기(Get) 시도
                        // GetObject(TriggerContainer) -> Container(Hand) 
                        if (handContainer.CanPut(GetObjectInTriggerContainer))
                        {
                            // GetObjectInTriggerContainer --> Food
                            if (triggerContainer.Peek().TryGetComponent<Container>(out Container InnerContainer))
                            {
                                InnerContainer.gameObject.DebugName("Inner Container");
                                InnerContainer.Get();
                            }
                            handContainer.TryPut(GetObjectInTriggerContainer);
                        }
                    }
                }
                else
                {
                    // Container(Trigger)가 물체를 보관하고 있지 않으면,
                    // 아래 2가지 경우의 수를 고려해야 함

                    // 1. Container(Hand) 자체를 넣기(Put)
                    // : Container(Hand) -> Container(Trigger)

                    // 2. 가져오고(Get) 넣기(Put)
                    // : GetObject(HandContainer) -> Container(Trigger)


                    // 1번. Container(Hand) 자체를 넣기(Put)

                    // Container(Hand)를 Container(Trigger)에 넣기(Put) 시도
                    // Container(Hand) -> Container(Trigger) 
                    if (triggerContainer.CanPut(handContainer))
                    {
                        hand.HoldOut();
                        triggerContainer.TryPut(handContainer);
                        handContainer.gameObject.DebugName("Put Container");
                    }
                    else
                    {
                        // 2번. 가져오고(Get) 넣기(Put)

                        // Container(Hand)가 물체를 보관하고 있는지 검사 
                        if(GetObjectInHandContainer != null)
                        {
                            // Container(Hand) 내의 물체를 꺼내서 Container(Trigger)로 넣기(Put)를 시도
                            // GetObject(HandContainer) -> Container(Trigger)
                            if (triggerContainer.CanPut(GetObjectInHandContainer))
                            {
                                handContainer.Get();
                                triggerContainer.TryPut(GetObjectInHandContainer);
                                GetObjectInHandContainer.gameObject.DebugName("Put Object");
                            }
                        }
                    }
                }
                
            }
            else if (objectType == EObjectType.Food)
            { 
                // 탐지된 물체가 음식이라면,
                // 해당 음식을 Container(Hand)에 넣을 수 있는 검사
                if (handContainer.CanPut(triggeredObject))
                {
                    handContainer.TryPut(triggeredObject);
                }
            }
            else
            {   // 바닥에 놓기
                hand.HoldOutAndFree();
            }
        }
        else
        {
            // 바닥에 놓기
            hand.HoldOutAndFree();
        }

        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {

    }
}
