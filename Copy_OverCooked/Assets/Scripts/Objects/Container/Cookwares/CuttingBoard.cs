using UnityEngine;

public class CuttingBoard : Cookware
{
    private void Update()
    {
        if(cookwareState == ECookwareState.Cook)
        {

        }
    }

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

    public override InteractableObject Get()
    {
        if(cookwareState == ECookwareState.Cook) StopCook();
        return base.Get();
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
