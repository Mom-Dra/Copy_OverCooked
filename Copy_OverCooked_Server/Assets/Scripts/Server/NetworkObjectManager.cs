using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectManager : MonobehaviorSingleton<NetworkObjectManager>
{
    private static int s_nextId = 1;
    private Dictionary<int, NetworkObject> objectDic;

    public Dictionary<int, NetworkObject>.ValueCollection ObjectDic { get => objectDic.Values; }

    protected override void Awake()
    {
        base.Awake();
        objectDic = new Dictionary<int, NetworkObject>();
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
        obj.SetId(s_nextId);
        objectDic.Add(s_nextId++, obj);
    }

    public NetworkObject GetObjectById(int id)
    {
        return objectDic[id];
    }

    
}
