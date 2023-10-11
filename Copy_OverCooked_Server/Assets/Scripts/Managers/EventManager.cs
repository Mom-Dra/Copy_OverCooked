using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonobehaviorSingleton<EventManager> 
{
    private IEnumerator checkCoroutine;

    private List<Event> events;

    protected override void Awake()
    {
        base.Awake();
        events = new List<Event>();
    }

    private IEnumerator CheckCoroutine()
    {
        while (true)
        {
            if (events.Count > 0)
            {
                for (int i = 0; i < events.Count; ++i)
                {
                    if (events[i].HasNextAction())
                    {
                        events[i].TryAction();
                    } else
                    {
                        RemoveEvent(events[i]);
                        --i;
                    }
                }
            } else
                break;

            yield return null;
        }
    }

    // 이벤트 삽입
    public void AddEvent(Event _event)
    {
        events.Add(_event);
        _event.AddEventAction();

        if (checkCoroutine == null)
        {
            checkCoroutine = CheckCoroutine();
            StartCoroutine(checkCoroutine);
        }
    }


    // 이벤트 삭제 
    public void RemoveEvent(Event _event)
    {
        events.Remove(_event);

        if (events.Count == 0)
        {
            StopCoroutine(checkCoroutine);
            checkCoroutine = null;
        }
    }
}