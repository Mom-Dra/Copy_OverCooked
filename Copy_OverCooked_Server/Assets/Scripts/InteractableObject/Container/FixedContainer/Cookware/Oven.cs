using UnityEngine;

public class Oven : Cookware
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryGet<IFood>(out IFood food))
            {
                TryCook();
            }
            return true;
        }
        return false;
    }

    protected override bool CanCook()
    {
        return TryGet<Tray>(out Tray tray);
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<Tray>(out Tray tray);
    }

    public override void Remove(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent<CookableTray>(out CookableTray cookableTray))
        {
            cookableTray.ParentCookware = null;
        }
        base.Remove(interactableObject);
    }

    public override void OnProgressBegin()
    {

    }

    public override void OnProgressEnd()
    {

    }
}