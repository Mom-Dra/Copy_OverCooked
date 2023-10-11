using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InteractManager : MonobehaviorSingleton<InteractManager> // 우리의 신 
{
    private Dictionary<Player, InteractableObject> connectionList = new Dictionary<Player, InteractableObject>();

    private int CompareType(InteractableObject host, InteractableObject guest)
    {
        return host.GetShownType() - guest.GetShownType();
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
            } else
            {
                MoveObject(targetContainer, hand);
            }
        } else
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
            case EObjectType.Empty_Fixed_Container:
                if (sender.TryGet<Tray>(out Tray tray, EGetMode.Pop))
                {
                    sendObject = tray;
                } else if (sender.TryGet<Food>(out Food food, EGetMode.Pop))
                {
                    sendObject = food;
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

            case EObjectType.Tray:
                if (sender.TryGet<Food>(out Food food1, EGetMode.Pop))
                {
                    sendObject = food1;
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
                if (sender.TryGet<Food>(out Food food2, EGetMode.Pop))
                {
                    sendObject = food2;
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