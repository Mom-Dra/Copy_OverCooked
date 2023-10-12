using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityMainThread : MonobehaviorSingleton<UnityMainThread>
{
    private Queue<UnityAction> actionQueue = new Queue<UnityAction>();

    private void Update()
    {
        if (actionQueue.Count > 0)
        {
            actionQueue.Dequeue().Invoke();
        }
    }

    public void AddJob(UnityAction job)
    {
        actionQueue.Enqueue(job);
    }
}
