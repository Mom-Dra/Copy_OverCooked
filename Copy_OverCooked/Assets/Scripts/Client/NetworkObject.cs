using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    [SerializeField]
    protected EObjectSerialCode serialCode;

    protected int id;

    // Property
    public int Id { get => id; set { id = value; } }
    public EObjectSerialCode ObjectSerialCode { get => serialCode; }

    private void Start()
    {
        NetworkObjectManager.Instance.Add(this);
    }
}