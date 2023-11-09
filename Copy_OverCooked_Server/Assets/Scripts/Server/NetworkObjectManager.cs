using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class NetworkObjectManager : MonobehaviorSingleton<NetworkObjectManager>
{
    public Dictionary<int, SerializedObject> networkObjectDic;

    private int playerCount = 0;

    public Dictionary<int, SerializedObject>.ValueCollection ObjectDic 
    {
        get => networkObjectDic.Values;
    }

    protected override void Awake()
    {
        base.Awake();
        networkObjectDic = new Dictionary<int, SerializedObject>();
    }

    public GameObject Instantiate(GameObject gameObject, Vector3 vector3, Quaternion quaternion)
    {
        GameObject instantiateObject = GameObject.Instantiate(gameObject, vector3, quaternion);
        return instantiateObject;
    }

    public void SpawnPlayer(int clientId)
    {
        Transform spawnLocation = GameObject.Find("PlayerSpawnPositions").transform.GetChild(playerCount++);
        GameObject player = this.Instantiate(SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Player), spawnLocation.position, spawnLocation.rotation);

        int targetId = player.GetComponent<SerializedObject>().Id;
        Debug.Log($" Player Id :{targetId}");
        PacketSend.SpawnPlayer(targetId, clientId);
    }

    public void Add(SerializedObject obj)
    {
        //Debug.Log($"Set ID : {obj.gameObject.name} -> {s_nextId}");
        //obj.Id = s_nextId;
        networkObjectDic.Add(obj.Id, obj);
    }

    public SerializedObject GetObjectById(int id)
    {
        return networkObjectDic[id];
    }
}
