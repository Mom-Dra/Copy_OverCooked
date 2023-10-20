using UnityEngine;

public class Fryer : Cookware
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryGet<Food>(out Food food))
            {
                TryCook();
            }
            return true;
        }
        return false;
    }
    protected override bool CanCook()
    {
        return TryGet<FryerTray>(out FryerTray tray);
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<FryerTray>(out FryerTray fryerTray);
    }
}