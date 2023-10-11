using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectManager : MonobehaviorSingleton<NetworkObjectManager>
{
    private static int s_nextId = 1;
    private Dictionary<int, NetworkObject> networkObjectDic;

    public Dictionary<int, NetworkObject>.ValueCollection ObjectDic { get => networkObjectDic.Values; }

    protected override void Awake()
    {
        base.Awake();
        networkObjectDic = new Dictionary<int, NetworkObject>();
    }

    public GameObject Instantiate(GameObject gameObject, Vector3 vector3, Quaternion quaternion)
    {
        GameObject instantiateObject = GameObject.Instantiate(gameObject, vector3, quaternion);

        if (!instantiateObject.GetComponent<NetworkObject>())
        {
            instantiateObject.AddComponent<NetworkObject>();
        }

        return instantiateObject;
    }

    public void Add(NetworkObject obj)
    {
        //Debug.Log($"Set ID : {obj.gameObject.name} -> {s_nextId}");
        obj.Id = s_nextId;
        networkObjectDic.Add(s_nextId++, obj);
    }

    public NetworkObject GetObjectById(int id)
    {
        return networkObjectDic[id];
    }

    
}
