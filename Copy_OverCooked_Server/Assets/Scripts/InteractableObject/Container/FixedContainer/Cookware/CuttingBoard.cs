using UnityEngine;

public class CuttingBoard : Cookware, Reactable
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
        Food getFood = getObject as Food;
        return cookwareState != ECookwareState.Cook || (cookwareState == ECookwareState.Cook && getFood.CurrOverTime < 5);
    }
}