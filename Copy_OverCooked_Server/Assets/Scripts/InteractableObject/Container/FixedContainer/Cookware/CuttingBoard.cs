using UnityEngine;

public class CuttingBoard : Cookware, IReactable
{
    public void React(Player player)
    {
        if (TryCook())
        {
            EventManager.Instance.AddEvent(new ChopEvent(player, this));
        }
    }

    protected override bool CanCook()
    {
        return true;
    }

    protected override bool CanGet()
    {
        IFood getFood = getObject as IFood;
        return cookwareState != ECookwareState.Cook || (cookwareState == ECookwareState.Cook && getFood.CurrCookingRate < 5);
    }

    public override void OnProgressBegin()
    {

    }

    public override void OnProgressEnd()
    {

    }
}