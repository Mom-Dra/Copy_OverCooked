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
        return host.GetTopType() - guest.GetTopType();
    }

    public void Match(Hand hand, InteractableObject target)
    {
        if (target.TryGetComponent<Container>(out Container targetContainer))
        // target이 Container인 경우
        {
            int compare = CompareType(hand, target);
            if (compare > 0 || (compare == 0 && (hand.GetTopType() == EObjectType.Food)))
            {
                MoveObject(hand, targetContainer);
            } else
            {
                MoveObject(targetContainer, hand);
            }
        } else
        // target이 바닥에 있는 음식인 경우 
        {
            if (!hand.TryPut(new SendContainerArgs(target, null))) // 오버로딩 필요 
            {
                hand.HoldOut();
            }
        }
    }

    public void MoveObject(Container sender, Container receiver)
    {
        Debug.Log($"<color=yellow> Move {sender.name} -> {receiver.name} </color>");

        EObjectType recvTopType = receiver.GetTopType();
        Container sendContainer = sender;

        if(!receiver.TryFind<FoodBox>(out FoodBox foodBox) && (recvTopType == EObjectType.Tray || recvTopType == EObjectType.Food))
        {
            sendContainer = sender.GetTopContainer();
        }

        SendContainerArgs sendContainerArgs = new SendContainerArgs(null, sendContainer);

        if (sendContainer.TryGet(out InteractableObject getObject))
        {
            sendContainerArgs.Item = getObject;
            if (receiver.TryPut(sendContainerArgs))
            {
                Debug.Log($"<color=yellow> OK </color>");
            } 
            else if(recvTopType == EObjectType.Food)
            {
                if (sender.TryFind<Hand>(out Hand hand))
                {
                    MoveObject(receiver, hand);
                } 
            }
        }

        //switch (topReceiveType)
        //{
        //    case EObjectType.Empty_Fixed_Container:

        //        break;

        //    case EObjectType.Tray:
        //        sendContainer = sender.GetTopContainer();
        //        break;

        //    case EObjectType.Food:
        //        if (sender.TryFind<Food>(out Food food2))
        //        {
        //            sendObject = food2;
        //        }

        //        if (sendObject != null)
        //        {
        //            if (receiver.TryPut(sendObject))
        //            {
        //                Debug.Log($"<color=yellow> OK </color>");
        //                sender.Remove(sendObject);
        //            } else
        //            {
        //                if (sender.TryFind<Hand>(out Hand hand))
        //                {
        //                    MoveObject(receiver, hand);
        //                }
        //            }
        //        }
        //        break;
        //}


    }
}