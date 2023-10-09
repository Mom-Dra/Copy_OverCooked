using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Event
{
    protected List<Func<bool>> actions = new List<Func<bool>>();
    protected int currIndex = 0;
    // CurrAction 변수로 놓자 
    // EventAction 을 배열로 만들고,
    // 각 Event에서 

    public bool HasNextAction()
    {
        return currIndex < actions.Count;
    }

    public void TryAction()
    {
        if (actions[currIndex]())
        {
            currIndex++;
        }
        // 현재 액션을 실행하고,
        // 실행하였다면 다음 액션을 현재 액션으로 등록 
    }
    public abstract void AddEventAction(); 
}