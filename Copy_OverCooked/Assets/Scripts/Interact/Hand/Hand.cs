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
        // 탐지된 오브젝트 가져오기
        InteractableObject triggeredObject = hand.TriggeredObject;
        // 오브젝트가 <Cookware> 스크립트를 가지고 있는지 확인

        // 가지고 있다면,
        if (triggeredObject.TryGetComponent<Cookware>(out Cookware cookware))
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
            // 음식 바닥에 놓기
            hand.HoldOut();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        // 던지는 코드
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
            // 조리도구 -> 접시 = 음식이 옮겨짐
            // 조리도구 -> 선반 = 조리도구가 옮겨짐 
            // 케이스 수정 바람 
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
            // 바닥에 놓기
            hand.HoldOut();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {

    }
}
