using System;
using System.Collections.Generic;
using UnityEngine;

public class PacketHandle
{
    private static Dictionary<int, Action<Packet>> actionDics;

    public static void Init()
    {
        actionDics = new Dictionary<int, Action<Packet>>()
        {
            { (int)EActionCode.Input, InputCommand },
            { (int)EActionCode.Event, EventCommand },
        };
    }

    public static void Invoke(Packet packet)
    {
        EActionCode actionCode = packet.actionCode;
        actionDics[(int)actionCode].Invoke(packet);
    }

    private static void InputCommand(Packet packet)
    {
        packet.Read(out int inputType);
        EInputType eInput = (EInputType)inputType;
        switch (eInput)
        {
            case EInputType.Move:
                Move(packet);
                break;
        }
    }

    private static void EventCommand(Packet packet)
    {
        int clientId = packet.targetId;
        packet.Read(out int eventType);
        EEventType eEventType = (EEventType)eventType;
        switch (eEventType)
        {
            case EEventType.SceneLoaded:
                PacketSend.UploadMapDataToClient(clientId);
                break;
        }
    }

    private static void Move(Packet packet)
    {
        int targetId = packet.targetId;
        packet.Read(out int clientId);
        packet.Read(out Vector2 direction);
        Player movePlayer = NetworkObjectManager.Instance.GetObjectById(targetId).GetComponent<Player>();
        movePlayer.SetMoveDirection(new Vector3(direction.x, 0f, direction.y));
    }

    




}
