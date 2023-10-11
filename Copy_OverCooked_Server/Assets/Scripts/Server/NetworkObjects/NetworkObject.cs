using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    protected int id;

    // Property
    public int Id { get => id; set { id = value; } }

    private void Start()
    {
        NetworkObjectManager.Instance.Add(this);
    }
}