using UnityEngine;

public class ChopEvent : Event
{
    private Player player;
    private Cookware cookware;

    public ChopEvent(Player player, Cookware cookware)
    {
        this.player = player;
        this.cookware = cookware;
        actions.Add(StopChopEventAction);
    }

    private bool StopChopEventAction()
    {
        if (CheckDistance())
        {
            if (cookware.cookwareState == ECookwareState.Cook)
            {
                cookware.StopCook();
            }
            player.SetBoolAnimation(EAnimationType.Chop, false);
            return true;
        }
        return false;
    }

    private bool CheckDistance()
    {
        float distance = Vector3.Distance(player.transform.position + player.transform.forward, cookware.transform.position);

        if (distance > 2f || cookware.cookwareState == ECookwareState.Complete)
        {
            return true;
        }
        return false;
    }

    public override void AddEventAction()
    {
        player.SetBoolAnimation(EAnimationType.Chop, true);
    }
}