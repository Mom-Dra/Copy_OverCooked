using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectManager : MonobehaviorSingleton<NetworkObjectManager>
{
    private static int s_nextId = 1;
    private Dictionary<int, NetworkObject> objectDic;

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

    public void UploadObjects(ClientHandler clientHandler)
    {
        foreach (NetworkObject obj in objectDic.Values)
        {
            using (Packet packet = new Packet(EActionCode.Instantiate, obj.GetId()))
            {
                packet.Write((int)EInstantiateType.Instantiate);
                packet.Write(obj.transform.position);
                packet.Write(obj.transform.rotation);
                clientHandler.Send(packet);
            }
        }
    }
}
