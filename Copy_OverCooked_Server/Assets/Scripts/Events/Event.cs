using System;
using System.Collections.Generic;


public abstract class Event
{
    protected List<Func<bool>> actions = new List<Func<bool>>();
    protected int currIndex = 0;

    public bool HasNextAction()
    {
        return currIndex < actions.Count;
    }

    public void TryAction()
    {
        if (actions[currIndex].Invoke())
        {
            currIndex++;
        }
    }
    public abstract void AddEventAction();
}