using UnityEngine;

public interface Containable
{
    public GameObject Get();
    public void Put(GameObject gameObject);
}