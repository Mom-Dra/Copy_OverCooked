using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractManager : MonobehaviorSingleton<InteractManager> 
{
    private int CompareType(InteractableObject host, InteractableObject guest)
    {
        return host.GetTopType() - guest.GetTopType();
    }

    public void Match(Hand hand, InteractableObject target)
    {
        if (target.TryGetComponent<Container>(out Container targetContainer))
        // target이 Container인 경우
        {
            int compare = CompareType(hand, target);
            EObjectType handTopType = hand.GetTopType();
            if (compare > 0 || (compare == 0 && (handTopType == EObjectType.Food || handTopType == EObjectType.Tray)))
            {
                MoveObject(hand, targetContainer);
            } else
            {
                MoveObject(targetContainer, hand);
            }
        } else
        // target이 바닥에 있는 음식인 경우 
        {
            if (!hand.TryPut(target)) // 오버로딩 필요 
            {
                hand.HoldOut();
            }
        }
    }

    public void MoveObject(Container sender, Container receiver)
    {
        Debug.Log($"<color=yellow> Move {sender.name} -> {receiver.name} </color>");

        EObjectType recvTopType = receiver.GetTopType();
        InteractableObject sendObject = null;

        switch (recvTopType)
        {
            case EObjectType.Empty_Fixed_Container:
                if (receiver.SerialCode != EObjectSerialCode.Trashcan && sender.TryGet<Tray>(out Tray tray, EGetMode.Pop))
                {
                    sendObject = tray;
                }
                else if (sender.TryGet<IFood>(out IFood food, EGetMode.Pop))
                {
                    //sendObject = food;
                    sendObject = food.GameObject.GetComponent<InteractableObject>();
                }
                else if(sender.TryGet<FireExtinguisher>(out FireExtinguisher fe, EGetMode.Pop))
                {
                    sendObject = fe;
                }

                if (sendObject != null)
                {
                    if (receiver.TryPut(sendObject))
                    { 
                        Debug.Log($"<color=yellow> OK </color>");
                        // 여기 수정해야 할 것 같은데 
                        if(sender.ObjectType != EObjectType.Tray)
                        {
                            sender.Remove(sendObject);
                        }
                    }
                }
                break;
            case EObjectType.Tray:
                if (sender.TryGet<IFood>(out IFood food1, EGetMode.Pop))
                {
                    sendObject = food1.GameObject.GetComponent<InteractableObject>();
                }else if(sender.TryGet<Tray>(out Tray tray2))
                {
                    sendObject = tray2;
                }

                if (sendObject != null)
                {
                    if (receiver.TryPut(sendObject))
                    {
                        Debug.Log($"<color=yellow> OK </color>");
                        sender.Remove(sendObject);
                    }
                }
                break;
            case EObjectType.Food:
                if (sender.TryGet<IFood>(out IFood food2, EGetMode.Pop))
                {
                    sendObject = food2.GameObject.GetComponent<InteractableObject>();
                }

                if (sendObject != null)
                {
                    if (receiver.TryPut(sendObject))
                    {
                        Debug.Log($"<color=yellow> OK </color>");
                        sender.Remove(sendObject);
                    } else
                    {
                        if (sender.TryGet<Hand>(out Hand sendHand))
                        {
                            MoveObject(receiver, sendHand);
                        }
                    }
                }
                break;
        }
    }
}