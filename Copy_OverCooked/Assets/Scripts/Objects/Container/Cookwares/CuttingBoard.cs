using UnityEngine;

public class CuttingBoard : Cookware
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // Update로 계속 판별하는 것은 낭비이니까
        // Interactor에 기능을 위임할 수 있음
        // OnTriggerExit으로 CuttingBoard가 나갈 때 아래 코드를 호출하면 된다 
        if(cookwareState == ECookwareState.Cook)
        {
            Interactor interactor = LinkManager.Instance.GetLinkedPlayer(this).GetInteractor();
            if(!interactor.ContainObject(this))
            {
                StopCook();
            }
        }
    }

    public override bool TryCook()
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
