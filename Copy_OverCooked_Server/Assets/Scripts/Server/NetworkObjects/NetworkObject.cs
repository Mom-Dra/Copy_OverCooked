using System.Runtime.CompilerServices;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    private static int s_id;

    [SerializeField]
    protected EObjectSerialCode serialCode;
    [SerializeField]
    protected int id; // For Debug

    // Property
    public int Id { get => id; }

    public EObjectSerialCode ObjectSerialCode 
    { 
        get => serialCode; 
    }

    protected virtual void Awake()
    {
        id = s_id++;
    }

    private void Start()
    {
        NetworkObjectManager.Instance.Add(this);
    }
}