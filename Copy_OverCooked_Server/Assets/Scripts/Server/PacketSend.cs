using System;
using System.Collections.Generic;
using UnityEngine;

public class PacketSend
{
    public static void UploadMapDataToClient(int clientId)
    {
        foreach (SerializedObject obj in NetworkObjectManager.Instance.ObjectDic)
        {
            InteractableObject interactableObject = obj as InteractableObject;

            using (Packet packet = new Packet(EActionCode.Instantiate, obj.Id))
            {
                packet.Write((int)EInstantiateType.Instantiate);
                packet.Write((int)obj.SerialCode);
                packet.Write(obj.transform.position);
                packet.Write(obj.transform.rotation);

                Server.Instance.SendToClient(packet, clientId);
            }
        }
    }

    public static void SpawnPlayer(int targetId, int clientId)
    {
        using (Packet packet = new Packet(EActionCode.Event, targetId))
        {
            packet.Write((int)EEventType.SpawnPlayer);

            Server.Instance.SendToClient(packet, clientId);
        }
    }
}
