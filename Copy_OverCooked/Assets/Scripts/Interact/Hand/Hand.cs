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
    private HandState handState;
    
    private Interactor interactor;

    public float throwPower;
    [HideInInspector]
    public InteractableObject CurrentObject;

    public InteractableObject TriggeredObject { get => interactor.GetTriggeredObject(); }

    private void Awake()
    {
        handState = EmptyHandState.Instance;

        interactor = transform.parent.GetComponentInChildren<Interactor>();
    }

    private void Update()
    {
        if (CurrentObject != null)
        {
            CurrentObject.transform.position = transform.position;
        }
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

    public void HoldIn(InteractableObject gameObject)
    {
        if (gameObject != null)
        {
            CurrentObject = gameObject;
            CurrentObject.IsInteractable = false;
            Rigidbody rigid = CurrentObject.GetComponent<Rigidbody>();
            rigid.rotation = Quaternion.Euler(0, 0, 0);
            rigid.isKinematic = true;
        }
    }

    public void HoldOut()
    {
        if (CurrentObject != null)
        {
            CurrentObject.IsInteractable = true;
            Rigidbody rigid = CurrentObject.GetComponent<Rigidbody>();
            rigid.isKinematic = false;
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
            }
            else
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

        // ������ �ִٸ�,
        if (triggeredObject.TryGetComponent<Cookware>(out Cookware cookware))
        {
            // �ش� ���������� ��ȣ�ۿ� (�丮)
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

        if (triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
            if (objectType == EObjectType.Container)
            {
                if (triggeredObject.GetComponent<Container>().Put(hand.CurrentObject))
                {
                    hand.HoldOut();
                }
            }
            else
            {
                hand.HoldOut();
            }
        }
        else
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
                if (getObject != null && getObject.tag == "Food")
                {
                    if (triggeredObject.GetComponent<Container>().Put(getObject))
                    {
                        hand.HoldOut();
                    }
                }
            }
            else
            {
                hand.HoldOut();
            }
        }
        else
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
