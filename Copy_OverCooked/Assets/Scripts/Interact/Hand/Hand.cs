using UnityEngine;
using UnityEngine.XR;

// �÷��̾��� �տ� �ִ� ������Ʈ�� ���� 
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

    public void HoldOutAndFree() // Free�� �� �� ���� ���� 
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
        // Ž���� ������Ʈ �������� 
        InteractableObject triggeredObject = hand.TriggeredObject;
        // Ž���� ������Ʈ�� �ִٸ�, 
        if (triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
            // ObjectType = Container ���,
            if (objectType == EObjectType.Container)
            {
                // Container���� ��ü�� �����ͼ� hand�� ���� 
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
        // Ž���� ������Ʈ ��������
        InteractableObject triggeredObject = hand.TriggeredObject;
        // ������Ʈ�� <Cookware> ��ũ��Ʈ�� ������ �ִ��� Ȯ�� 
        if (triggeredObject != null)
        {
            // ������ �ִٸ�, 
            if (triggeredObject.TryGetComponent<Cookware>(out Cookware cookware))
            {
                // �ش� ���������� ��ȣ�ۿ� (�丮) 
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
                // ���� �ٴڿ� ���� 
                hand.HoldOutAndFree();
            }
        } else
        {
            // ���� �ٴڿ� ���� 
            hand.HoldOutAndFree();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        // ������ �ڵ�
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
        // �տ� ��� �ִ� Container(Hand)
        Container handContainer = hand.CurrentObject.GetComponent<Container>();
        
        // Ž���� ��ü�� Null���� �˻� 
        InteractableObject triggeredObject = hand.TriggeredObject;
        if (triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
            // Ž���� ��ü�� Container���, 
            // Container(Hand) -> Container(Triggered) �Ǵ�
            // Container(Hand) <- Container(Triggered) �̷� ������ �� 
            if (objectType == EObjectType.Container)
            {
                // Ž���� Container(Trigger)
                Container triggerContainer = triggeredObject.GetComponent<Container>();

                // Container(Hand) ���� ��ü GetObject(HandContainer)
                InteractableObject GetObjectInHandContainer = handContainer.PeekGetObject();
                // Container(Trigger) ���� ��ü GetObjet(TriggerContainer)
                InteractableObject GetObjectInTriggerContainer = triggerContainer.PeekGetObject();

                // Container(Trigger)�� ��ü�� �����ϰ� �ִ��� �˻�
                if (GetObjectInTriggerContainer != null)
                {
                    // Container(Trigger)�� ��ü�� �����ϰ� �ִٸ�,
                    // �Ʒ� 2���� ����� ���� ����ؾ� �� 

                    // 1. �ֱ�(Put)
                    // : Container(Trigger)�� �߰��� �ֱ� 

                    // 2. ��������(Get)
                    // : Container(Trigger) ���� ��ü�� ������ Container(Hand)�� �ֱ�


                    // 1��. �ֱ�(Put)

                    // GetObject(HandContainer) -> Container(Trigger)
                    if (GetObjectInHandContainer != null && triggerContainer.CanPut(GetObjectInHandContainer))
                    {
                        handContainer.Get();
                        triggerContainer.TryPut(GetObjectInHandContainer);
                    }
                    else
                    {
                        // 2��. ��������(Get) 

                        // Container(Trigger) ���� ��ü�� ������ Container(Hand)�� ��������(Get) �õ�
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
                    // Container(Trigger)�� ��ü�� �����ϰ� ���� ������,
                    // �Ʒ� 2���� ����� ���� ����ؾ� ��

                    // 1. Container(Hand) ��ü�� �ֱ�(Put)
                    // : Container(Hand) -> Container(Trigger)

                    // 2. ��������(Get) �ֱ�(Put)
                    // : GetObject(HandContainer) -> Container(Trigger)


                    // 1��. Container(Hand) ��ü�� �ֱ�(Put)

                    // Container(Hand)�� Container(Trigger)�� �ֱ�(Put) �õ�
                    // Container(Hand) -> Container(Trigger) 
                    if (triggerContainer.CanPut(handContainer))
                    {
                        hand.HoldOut();
                        triggerContainer.TryPut(handContainer);
                        handContainer.gameObject.DebugName("Put Container");
                    }
                    else
                    {
                        // 2��. ��������(Get) �ֱ�(Put)

                        // Container(Hand)�� ��ü�� �����ϰ� �ִ��� �˻� 
                        if(GetObjectInHandContainer != null)
                        {
                            // Container(Hand) ���� ��ü�� ������ Container(Trigger)�� �ֱ�(Put)�� �õ�
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
                // Ž���� ��ü�� �����̶��,
                // �ش� ������ Container(Hand)�� ���� �� �ִ� �˻�
                if (handContainer.CanPut(triggeredObject))
                {
                    handContainer.TryPut(triggeredObject);
                }
            }
            else
            {   // �ٴڿ� ����
                hand.HoldOutAndFree();
            }
        }
        else
        {
            // �ٴڿ� ����
            hand.HoldOutAndFree();
        }

        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {

    }
}
