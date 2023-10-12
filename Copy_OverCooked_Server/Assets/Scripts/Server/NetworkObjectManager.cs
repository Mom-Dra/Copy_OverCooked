using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class NetworkObjectManager : MonobehaviorSingleton<NetworkObjectManager>
{
    private static int s_nextId = 1;
    public Dictionary<int, NetworkObject> networkObjectDic;

    public Dictionary<int, NetworkObject>.ValueCollection ObjectDic 
    {
        get
        {
            //try
            //{
            //    Debug.Log($"Dic COunt: {networkObjectDic.Count}");
            //    Debug.Log($"Linq : {networkObjectDic.Values}");
            //    foreach (KeyValuePair<int, NetworkObject> value in networkObjectDic)
            //    {
            //        Debug.Log("Nt OB: " + value.Value);
            //    }
            //}
            //catch (Exception e)
            //{
            //    Debug.LogException(e);
            //}
            
            return networkObjectDic.Values;
        } 
    }

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
        networkObjectDic.Add(s_nextId, obj);
        //Debug.Log($"NetworkObject Add : {networkObjectDic[s_nextId].name}");
        s_nextId++;
    }

    public NetworkObject GetObjectById(int id)
    {
        return networkObjectDic[id];
    }

    
}
