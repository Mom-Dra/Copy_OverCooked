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
        return true;
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<Tray>(out Tray tray);
    }

    public override void OnProgressBegin()
    {

    }

    public override void OnProgressEnd()
    {

    }
}