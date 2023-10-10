using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    protected int id;

    public int GetId()
    {
        return id;
    }

    public void SetId(int id)
    {
        this.id = id;
    }
}