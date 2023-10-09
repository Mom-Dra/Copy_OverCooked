using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PacketHandler
{
    private static Dictionary<int, Action<Packet>> actionDics;

    public static void Init()
    {
        actionDics = new Dictionary<int, Action<Packet>>()
        {
            { (int)EActionCode.Input, Test },
            { (int)EActionCode.Event, Test2 },
            { (int)EActionCode.Transform, Test },
            { (int)EActionCode.Instantiate, Test },
            { (int)EActionCode.Active, Test },
            { (int)EActionCode.Animation, Test },
            { (int)EActionCode.Sound, Test },
        };
    }

    public static void Invoke(Packet packet)
    {
        EActionCode actionCode = packet.actionCode;
        actionDics[(int)actionCode].Invoke(packet);
    }

    private static void Test(Packet packet)
    {
        Debug.Log("Test");
    }

    private static void Test2(Packet packet)
    {
        Debug.Log("Test2");
    }
}
