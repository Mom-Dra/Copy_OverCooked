using UnityEngine;

public class CuttingBoard : Cookware, Reactable
{
    public void React(Player player) 
    {
        // �÷��̾��� Animation ȣ��

        EventManager.Instance.AddEvent(new ChopEvent(player, this));
        TryCook();
    }

    protected override bool CanCook()
    {
        if(TryGet<Food>(out Food food))
        {
            
        }
        return true;
    }
}