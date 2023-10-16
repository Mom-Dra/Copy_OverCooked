using System.Collections.Generic;
using UnityEngine;

public class Induction : Cookware
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryFind<Food>(out Food food))
            {
                TryCook();
            }
            return true;
        }
        return false;
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return base.IsValidObject(interactableObject);
    }

    protected override bool CanCook()
    {
        return true;
    }
}