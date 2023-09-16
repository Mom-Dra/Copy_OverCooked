using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    private Container container;

    private void Awake()
    {
        container = transform.parent.GetComponentInChildren<Container>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Food>(out Food food))
        {
            if (food.IsInteractable && container.CanPut(food))
            {
                food.gameObject.DebugName(EDebugColor.Red, "ThrowPut");
                container.Put(food);
            }
        }
    }
}