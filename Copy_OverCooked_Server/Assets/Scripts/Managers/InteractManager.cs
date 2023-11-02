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
        // target�� Container�� ���
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
        // target�� �ٴڿ� �ִ� ������ ��� 
        {
            if (!hand.TryPut(target)) // �����ε� �ʿ� 
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
                        // ���� �����ؾ� �� �� ������ 
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