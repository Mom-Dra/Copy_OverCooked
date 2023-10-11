using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    protected int id;

    // Property
    public int Id { get => id; set { id = value; } }
}