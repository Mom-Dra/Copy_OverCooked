using UnityEngine;

public class Induction : Cookware
{
    public override bool TryPut(SendContainerArgs sendContainerArgs)
    {
        if (base.TryPut(sendContainerArgs))
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
        return base.IsValidObject(interactableObject) && interactableObject.TryFind<FryingPan>(out FryingPan value);
    }

    protected override bool CanCook()
    {
        return true;
    }
}