using UnityEngine;

public class ContainerTriggerBox : MonoBehaviour
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
                food.gameObject.DebugName("ThrowPut", EDebugColor.Red);
                container.TryPut(food);
            }
        }
    }
}