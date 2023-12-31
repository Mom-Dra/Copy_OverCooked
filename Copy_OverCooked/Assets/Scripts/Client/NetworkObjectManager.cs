using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectManager : MonobehaviorSingleton<NetworkObjectManager>
{
    private static int s_nextId = 1;
    private Dictionary<int, NetworkObject> networkObjectDic;

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
        networkObjectDic.Add(obj.Id, obj);
    }

    public NetworkObject FindById(int id)
    {
        return networkObjectDic[id];
    }

}
