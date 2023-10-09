using UnityEngine;

public class ChopEvent : Event
{
    private Player player;
    private Cookware reactableObject;

    public ChopEvent(Player player, Cookware reactableObject)
    {
        this.player = player;
        this.reactableObject = reactableObject;
    }

    public void Action() 
    {
        player.SetBoolAnimation(EAnimationType.Chop, false);
        reactableObject.StopCook();
    }

    public bool Condition()
    {
        float distance = Vector3.Distance(player.transform.position + player.transform.forward, reactableObject.transform.position);
        
        if(distance > 2f || reactableObject.cookwareState == ECookwareState.Complete)
        {
            return true;
        }

        return false;
    }

    public void AddEventAction()
    {
        player.SetBoolAnimation(EAnimationType.Chop, true);
    }

}