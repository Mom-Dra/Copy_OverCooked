using UnityEngine;

public class Induction : Cookware
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
}