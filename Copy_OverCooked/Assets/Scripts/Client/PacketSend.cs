using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public static class PacketSend
{
    public static void SceneLoad(string sceneName)
    {
        using (Packet packet = new Packet(EActionCode.Event, NetworkManager.Instance.ClientId)) 
        {
            packet.Write((int)EEventType.SceneLoaded);
            packet.Write(sceneName);

            NetworkManager.Instance.Send(packet);
        }
    }

    public static void Move(Vector2 inputVector2, int targetId)
    {
        using (Packet packet = new Packet(EActionCode.Input, targetId))
        {
            packet.Write((int)EInputType.Move);
            packet.Write(inputVector2);

            NetworkManager.Instance.Send(packet);
        }
    }
}
