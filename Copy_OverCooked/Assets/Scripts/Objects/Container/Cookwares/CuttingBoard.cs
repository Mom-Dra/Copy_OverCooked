using UnityEngine;

public class CuttingBoard : Cookware
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(cookwareState == ECookwareState.Cook)
        {
            Interactor interactor = LinkManager.Instance.GetLinkedPlayer(this).GetInteractor();
            if(!interactor.ContainObject(this))
            {
                StopCook();
            }
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

    public override void Fit(InteractableObject interactableObject)
    {
        interactableObject.Fix();
        interactableObject.transform.position = transform.position + containOffset;
    }

    public override bool IsValidObject(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<Food>(out Food food))
        {
            if(food.cookingMethod == ECookingMethod.Chop)
            {
                return true;
            }
        }
        return false;
    }

   
}
