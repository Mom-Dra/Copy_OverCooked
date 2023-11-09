using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    [SerializeField]
    protected EObjectSerialCode serialCode;

    protected int id;

    // Property
    public int Id { get => id; set { id = value; NetworkObjectManager.Instance.Add(this); } }
    public EObjectSerialCode ObjectSerialCode { get => serialCode; }
}