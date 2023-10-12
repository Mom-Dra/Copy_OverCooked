using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    [SerializeField]
    protected EObjectSerialCode serialCode;
    [SerializeField]
    protected int id; // For Debug

    // Property
    public int Id 
    { 
        get => id; 
        set 
        { 
            id = value; 
        } 
    }

    public EObjectSerialCode ObjectSerialCode 
    { 
        get => serialCode; 
    }

    protected virtual void Start()
    {
        NetworkObjectManager.Instance.Add(this);
    }
}