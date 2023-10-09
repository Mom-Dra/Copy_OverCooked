using UnityEngine;

public class FireTriggerBox : MonoBehaviour
{
    private ParticleSystem fire;

    private void Awake()
    {
        fire = transform.parent.GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent<Food>(out Food food))
        //{
            
        //}
    }
}