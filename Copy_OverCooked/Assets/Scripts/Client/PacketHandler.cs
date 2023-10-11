using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class PacketHandler
{
    private static Dictionary<int, Action<Packet>> actionDic;

    public static void Init()
    {
        actionDic = new Dictionary<int, Action<Packet>>()
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
        EActionCode actionCode = packet.ActionCode;
        actionDic[(int)actionCode].Invoke(packet);
    }

    private static void EventCommand(Packet packet)
    {

    }

    private static void TranformCommand(Packet packet)
    {
        int targetId = packet.TargetId;
        packet.Read(out Vector3 position);
        packet.Read(out Quaternion rotation);
        packet.Read(out Vector3 scale);
    }

    private static void InstantiateCommand(Packet packet)
    {
        int targetId = packet.TargetId;
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
