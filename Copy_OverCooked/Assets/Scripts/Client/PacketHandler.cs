using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class PacketHandler
{
    private static Dictionary<int, Action<Packet>> actionDics;

    public static void Init()
    {
        actionDics = new Dictionary<int, Action<Packet>>()
        {
            { (int)EActionCode.Event, EventCommand },
            { (int)EActionCode.Transform, TranformCommand },
            { (int)EActionCode.Instantiate, InstantiateCommand },
            { (int)EActionCode.Active, ActiveCommand },
            { (int)EActionCode.Animation, AnimationCommand },
            { (int)EActionCode.Sound, SoundCommand },
        };
    }

    public static void Invoke(Packet packet)
    {
        EActionCode actionCode = packet.actionCode;
        actionDics[(int)actionCode].Invoke(packet);
    }

    private static void EventCommand(Packet packet)
    {

    }

    private static void TranformCommand(Packet packet)
    {
        int targetId = packet.targetId;
        packet.Read(out Vector3 position);
        packet.Read(out Quaternion rotation);
        packet.Read(out Vector3 scale);
    }

    private static void InstantiateCommand(Packet packet)
    {
        int targetId = packet.targetId;
        packet.Read(out int type); // 1 : instantiate, 2 : Destroy
        if ((EInstantiateType)type == EInstantiateType.Instantiate)
        {
            packet.Read(out int serialCode);
            packet.Read(out Vector3 position);
            packet.Read(out Quaternion rotation);
            // <책갈피>
            
        }
        else
        {
            // 삭제 
        }
    }

    private static void ActiveCommand(Packet packet)
    {

    }

    private static void AnimationCommand(Packet packet)
    {

    }

    private static void SoundCommand(Packet packet)
    {

    }
}
