using UnityEngine;

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
        }
    }

    public void HoldOut() // Free�� �� �� ���� ���� 
    {
        if (CurrentObject != null)
        {
            CurrentObject.Free();

            CurrentObject.IsInteractable = true;
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
            // Ž���� ������Ʈ�� ObjectType �˻� (�̰� tag�� �ص� �ǰڳ� // ? )
            EObjectType objectType = triggeredObject.GetObjectType();
            // ObjectType = Container ���,
            if (objectType == EObjectType.Container)
            {
                // Container���� ��ü�� �����ͼ� hand�� ���� 
                InteractableObject getObject = triggeredObject.GetComponent<Container>().Get();
                hand.HoldIn(getObject);
                getObject.gameObject.DebugName(EDebugColor.Orange ,"Hand Get : ");
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
                    container.Put(io);
                }
            } else
            {
                // ���� �ٴڿ� ����
                hand.HoldOut();
            }
        } else
        {
            // ���� �ٴڿ� ����
            hand.HoldOut();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        // ������ �ڵ�
        InteractableObject io = hand.CurrentObject;
        hand.HoldOut();
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
        InteractableObject triggeredObject = hand.TriggeredObject;
        if (triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
            // �������� -> ���� = ������ �Ű���
            // �������� -> ���� = ���������� �Ű��� 
            // ���̽� ���� �ٶ� 
            if (objectType == EObjectType.Container)
            {
                InteractableObject getObject = hand.CurrentObject.GetComponent<Container>().Get();
                if (getObject != null && getObject.GetObjectType() == EObjectType.Food)
                {
                    Container container = triggeredObject.GetComponent<Container>();

                    if (container.CanPut(getObject))
                    {
                        hand.HoldOut();
                        container.Put(getObject);
                    }
                }
            } else
            {
                hand.HoldOut();
            }
        } else
        {
            // �ٴڿ� ����
            hand.HoldOut();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {

    }
}
