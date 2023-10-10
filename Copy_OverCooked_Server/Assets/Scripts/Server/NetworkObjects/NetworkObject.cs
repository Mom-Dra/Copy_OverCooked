using UnityEngine;

public class NetworkObject : MonoBehaviour
{
    protected int id;

    private void Start()
    {
        NetworkObjectManager.Instance.Add(this);
    }

    public int GetId()
    {
        return id;
    }

    public void SetId(int id)
    {
        this.id = id;
    }
}