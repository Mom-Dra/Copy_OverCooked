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
        float distance = Vector3.Distance(player.transform.position, reactableObject.transform.position);
        Debug.Log($"Distance : {distance}");
        if(distance > 3)
        {
            return true;
        }
        return false;
    }

    public void StartChop()
    {
        player.SetBoolAnimation(EAnimationType.Chop, true);
    }
}