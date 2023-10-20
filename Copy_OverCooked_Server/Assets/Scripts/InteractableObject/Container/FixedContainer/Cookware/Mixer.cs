using UnityEngine;

public class Mixer : Cookware
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
        return true;
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<MixerTray>(out MixerTray tray);
    }
}