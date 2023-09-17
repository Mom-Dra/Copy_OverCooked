using UnityEngine;

public class CuttingBoard : Cookware
{

    protected override bool TryCook()
    {
        if (base.TryCook())
        {
            LinkManager.Instance.GetLinkedPlayer(this).SetBoolAnimation(EAnimationType.Chop, true);
            return true;
        }
        return false;
    }

    protected override void StopCook()
    {
        LinkManager.Instance.GetLinkedPlayer(this).SetBoolAnimation(EAnimationType.Chop, false);
        base.StopCook();
    }

    public override void Fit(InteractableObject interactableObject)
    {
        interactableObject.Fix();
        interactableObject.transform.position = transform.position + containOffset;
    }

    public override bool IsValidObject(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<Food>(out Food food))
        {
            if(food.GetCookingMethod() == CookingMethod.Chop)
            {
                return true;
            }
        }
        return false;
    }

   
}
