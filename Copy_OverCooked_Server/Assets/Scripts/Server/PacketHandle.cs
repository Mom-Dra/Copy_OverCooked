using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class PacketHandle
{
    private static Dictionary<int, UnityAction<Packet>> actionDics;

    public static void Init()
    {
        actionDics = new Dictionary<int, UnityAction<Packet>>()
        {
            { (int)EActionCode.Input, OnInputAction },
            { (int)EActionCode.Event, OnEventAction },
        };
    }

    public static void Invoke(Packet packet)
    {
        EActionCode actionCode = packet.ActionCode;
        actionDics[(int)actionCode].Invoke(packet);
    }

    private static void OnInputAction(Packet packet)
    {
        packet.Read(out int inputType);
        switch ((EInputType)inputType)
        {
            case EInputType.Move:
                Move(packet);
                break;
        }
    }

    private static void OnEventAction(Packet packet)
    {
        packet.Read(out int eventType);
        switch ((EEventType)eventType)
        {
            case EEventType.SceneLoad:
                UploadMapData(packet);
                break;
        }
    }

    private static void Move(Packet packet)
    {
        int targetId = packet.TargetId;
        packet.Read(out Vector2 direction);
        Player movePlayer = NetworkObjectManager.Instance.GetObjectById(targetId).gameObject.GetComponent<Player>();
        Debug.Log($"Direction : {direction}, Player : {movePlayer}");
        movePlayer.SetMoveDirection(new Vector3(direction.x, 0f, direction.y));
    }

    private static void UploadMapData(Packet packet)
    {
        PacketSend.UploadMapDataToClient(packet.TargetId);
    }


}
