using UnityEngine;

public class DishwashingEvent : Event
{
    private Player player;
    private Sink sink;

    public DishwashingEvent(Player player, Sink sink)
    {
        this.player = player;
        this.sink = sink;
        actions.Add(StopDishwashingEvent);
    }

    private bool StopDishwashingEvent()
    {
        if (CheckDistance())
        {
            sink.StopSelectedCoroutine();
            //player.SetBoolAnimation(EAnimationType.Dishwash, false);
            return true;
        }
        return false;
    }

    private bool CheckDistance()
    {
        float distance = Vector3.Distance(player.transform.position + player.transform.forward, sink.transform.position);

        if (distance > 2f || !sink.HasDirtyPlate)
        {
            return true;
        }
        return false;
    }


    public override void AddEventAction()
    {
        //player.SetBoolAnimation(EAnimationType.Dishwash, true);
    }
}