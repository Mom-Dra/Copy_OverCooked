using System.Reflection;
using System.Transactions;
using TMPro;
using UnityEngine;

public class InteractManager : MonoBehaviour // 우리의 신 
{
    private static InteractManager instance;
    public static InteractManager Instance
    {
        get => instance;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }

    public void Match(Hand hand, InteractableObject target)
    {
        if (target.TryGetComponent<Container>(out Container targetContainer))
        // target이 Container인 경우
        {
            int compare = CompareType(hand, target);
            if (compare > 0 || (compare == 0 && (hand.GetShownType() == EObjectType.Food)))
            {
                MoveObject(hand, targetContainer);
            }
            else
            {
                MoveObject(targetContainer, hand);
            }
        }
        else
        // target이 바닥에 있는 음식인 경우 
        {
            if (!hand.TryPut(target))
            {
                hand.HoldOut();
            }
        }
    }

    public void MoveObject(Container sender, Container receiver) 
    {
        Debug.Log($"<color=yellow> Move {sender.name} -> {receiver.name} </color>");
        InteractableObject sendObject = null; 

        EObjectType topReceiveType = receiver.GetShownType();
        switch (topReceiveType)
        {
            case EObjectType.Empty:
                if(sender.TryGet<Tray>(out Tray tray))
                {
                    sendObject = tray;
                }
                else if(sender.TryGet<Food>(out Food food))
                {
                    sendObject = food;
                }

                if(sendObject != null)
                {
                    if (receiver.TryPut(sendObject))
                    {
                        sender.Remove(sendObject);
                    }
                }
                break;

            case EObjectType.Tray:
                if (sender.TryGet<Food>(out Food food1))
                {
                    sendObject = food1;
                }

                if (sendObject != null)
                {
                    if (receiver.TryPut(sendObject))
                    {
                        sender.Remove(sendObject);
                    }
                }
                break;

            case EObjectType.Food:
                if (sender.TryGet<Food>(out Food food2))
                {
                    sendObject = food2;
                }

                if (sendObject != null)
                {
                    if (receiver.TryPut(sendObject))
                    {
                        sender.Remove(sendObject);
                    }
                    else
                    {
                        if(sender.TryGet<Hand>(out Hand sendHand))
                        {
                            MoveObject(receiver, sendHand);
                        }
                    }
                }
                break;
        }
    }

    private int CompareType(InteractableObject host, InteractableObject guest)
    {
        return host.GetShownType() - guest.GetShownType();
    }
}