using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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
    [HideInInspector]
    public InteractableObject CurrentObject;

    public InteractableObject TriggeredObject { get =>interactor.GetTriggeredObject(); }

    private void Awake()
    {
        handState = EmptyHandState.Instance;

        interactor = GetComponent<Interactor>();
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
    }

    public void GrabAndPut()
    {
        handState.GrabAndPut(this);
    }

    public void InteractAndThorw()
    {
        handState.InteractAndThorw(this);
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
        InteractableObject triggeredObject = hand.TriggeredObject;
        if (triggeredObject != null)
        {
            EObjectType objectType = triggeredObject.GetObjectType();
                if (objectType == EObjectType.Container)
                {
                    GameObject getObject = triggeredObject.GetComponent<Containable>().Get();
                    hand.CurrentObject = getObject.GetComponent<InteractableObject>();
                } else
                {
                    hand.CurrentObject = triggeredObject;
                }

                hand.UpdateState();
        }
    }
    public override void InteractAndThorw(Hand hand)
    {
        // �Ʒ��� �ڵ尡 ���� ���� �ȵ� (���� ���� ���� ���)

        // Ž���� ������Ʈ ��������
        InteractableObject triggeredObject = hand.TriggeredObject;
        // ������Ʈ�� <Cookware> ��ũ��Ʈ�� ������ �ִ��� Ȯ��
        Cookware cookware = triggeredObject.GetComponent<Cookware>();
        // ������ �ִٸ�,
        if(cookware != null)
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
                    hand.CurrentObject = null;
                }
            } else
            {
                hand.CurrentObject = null;
            }
        }
        else
        {
            // ���� �ٴڿ� ����
            hand.CurrentObject = null;
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
            if (objectType == EObjectType.Container)
            {
                GameObject getObject = hand.CurrentObject.GetComponent<Containable>().Get();
                if (getObject != null && getObject.tag == "Food")
                {
                    if (triggeredObject.GetComponent<Containable>().Put(getObject))
                    {
                        hand.CurrentObject = null;
                    }
                }
            } else
            {
                hand.CurrentObject = null;
            }
        }
        else
        {
            // �ٴڿ� ����
            hand.CurrentObject = null;
        }
        hand.UpdateState();
    }

    public override void InteractAndThorw(Hand hand)
    {
        
    }
}
