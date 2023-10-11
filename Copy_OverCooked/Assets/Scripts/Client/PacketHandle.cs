using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class PacketHandle
{
    private static Dictionary<int, Action<Packet>> actionDic;

    public static void Init()
    {
        actionDic = new Dictionary<int, Action<Packet>>()
        {
            { (int)EActionCode.Event, OnEventHandle },
            { (int)EActionCode.Transform, OnTransformHandle },
            { (int)EActionCode.Instantiate, OnInstantiateHandle },
            { (int)EActionCode.Active, OnActiveHandle },
            { (int)EActionCode.Animation, OnAnimationHandle },
            { (int)EActionCode.Sound, OnSoundHandle },
        };
    }

    public static void Invoke(Packet packet)
    {
        actionDic[(int)packet.ActionCode].Invoke(packet);
    }

    private static void OnEventHandle(Packet packet)
    {

    }

    private static void OnTransformHandle(Packet packet)
    {
        NetworkObject go = NetworkObjectManager.Instance.FindById(packet.TargetId);
        if(go != null)
        {
            packet.Read(out Vector3 position);
            packet.Read(out Quaternion rotation);
            packet.Read(out Vector3 scale);
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.transform.localScale = scale;
        } 
        else
        {
            Debug.Log($"오브젝트를 찾지 못했습니다. Target ID : {packet.TargetId}");
        }
        
    }

    private static void OnInstantiateHandle(Packet packet)
    {
        packet.Read(out int type); 
        switch ((EInstantiateType)type)
        {
            case EInstantiateType.Instantiate:
                DoInstantiate(packet);
                break;
            case EInstantiateType.Destroy:
                DoDestroy(packet);
                break;
        }
    }

    private static void OnActiveHandle(Packet packet)
    {

    }

    private static void OnAnimationHandle(Packet packet)
    {

    }

    private static void OnSoundHandle(Packet packet)
    {

    }

    private static void DoInstantiate(Packet packet)
    {
        packet.Read(out int serialCode);
        packet.Read(out Vector3 position);
        packet.Read(out Quaternion rotation);
        GameObject instantiateGO = SerialCodeDictionary.Instance.FindBySerialCode((EObjectSerialCode)serialCode);
        instantiateGO.GetComponent<NetworkObject>().Id = packet.TargetId;
        GameObject.Instantiate(instantiateGO, position, rotation);
    }

    private static void DoDestroy(Packet packet)
    {
        GameObject.Destroy(NetworkObjectManager.Instance.FindById(packet.TargetId));
    }
}
