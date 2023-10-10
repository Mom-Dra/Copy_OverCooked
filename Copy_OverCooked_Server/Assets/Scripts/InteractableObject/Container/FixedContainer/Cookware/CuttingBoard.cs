using UnityEngine;

public class CuttingBoard : Cookware, Reactable
{
    public void React(Player player)
    {
        // �÷��̾��� Animation ȣ��
        if (TryCook())
        {
            EventManager.Instance.AddEvent(new ChopEvent(player, this));
        }
    }

    protected override bool CanCook()
    {
        //if(TryGet<Food>(out Food food))
        //{

        //}
        return true;
    }

    protected override bool CanGet()
    {
        Food getFood = getObject as Food;
        return cookwareState != ECookwareState.Cook || getFood.currCookDegree < 10;
    }
}