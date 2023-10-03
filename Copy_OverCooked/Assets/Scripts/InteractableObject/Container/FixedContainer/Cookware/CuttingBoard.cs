using UnityEngine;

public class CuttingBoard : Cookware, Reactable
{
    protected override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
    }

    public void React(Player player) 
    {
        ChopEvent chopEvent = new ChopEvent(player, this);
        chopEvent.StartChop();
        EventManager.Instance.AddEvent(chopEvent);
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