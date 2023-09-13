using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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
        // Ž���� ������Ʈ ��������
        InteractableObject triggeredObject = hand.TriggeredObject;
        // ������Ʈ�� <Cookware> ��ũ��Ʈ�� ������ �ִ��� Ȯ��
        
        // ������ �ִٸ�,
        if(triggeredObject.TryGetComponent<Cookware>(out Cookware cookware))
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
            // ���� �ٴڿ� ����
            hand.PutAway();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        // ������ �ڵ�
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
            // �������� -> ���� = ������ �Ű���
            // �������� -> ���� = ���������� �Ű��� 
            // ���̽� ���� �ٶ� 
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
            // �ٴڿ� ����
            hand.PutAway();
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        
    }
}
