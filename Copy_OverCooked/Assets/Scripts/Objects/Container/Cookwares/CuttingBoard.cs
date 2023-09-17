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

    public override InteractableObject Get()
    {
        if(cookwareState == ECookwareState.Cook) StopCook();
        return base.Get();
    }

    public override void Fit(InteractableObject interactableObject)
    {
        interactableObject.Fix();
        interactableObject.transform.position = transform.position + offset;
    }

    public override bool IsValidObject(InteractableObject interactableObject)
    {
        return interactableObject.GetObjectType() == EObjectType.Food;
    }
}
