using UnityEngine;

public interface Containable
{
    public GameObject Get();
    public bool Put(GameObject gameObject);
}