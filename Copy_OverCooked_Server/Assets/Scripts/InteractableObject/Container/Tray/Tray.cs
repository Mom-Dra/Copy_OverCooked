using UnityEngine;
using UnityEngine.UI;

public class Tray : Container
{
    [Header("Tray")]
    [SerializeField]
    private bool PlusBaseUI = true;

    protected override void Awake()
    {
        if (PlusBaseUI)
        {
            uIComponent = new BaseUIComponent(transform, uIOffset, maxContainCount);
        }
        base.Awake();
    }

    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + displayOffset;
        }
        if (uIComponent.HasImage)
        {
            uIComponent.OnImagePositionUpdate();
        }
    }

    public override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
        if(interactableObject.UIComponent.HasImage )
        {
            interactableObject.UIComponent.Clear();
        }
        if(interactableObject.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
        {
            Food food = interactableObject as Food;
            uIComponent.Add(InstantiateManager.Instance.InstantiateOnCanvas(food.GetFoodImage()));
        }
    }
}
