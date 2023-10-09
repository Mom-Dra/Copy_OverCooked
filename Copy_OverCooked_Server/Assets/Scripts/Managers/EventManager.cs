using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : MonobehaviorSingleton<EventManager>
{
    // EventManager = Event 관리 
    // 1. 이벤트 멤버들
    // 2. 이벤트 조건
    // 3. 이벤트 행동 

    private IEnumerator checkCoroutine;
    private bool isPlaying;

    private List<Event> events;

    protected override void Awake()
    {
        base.Awake();

        events = new List<Event>();
    }

    private IEnumerator CheckCoroutine()
    {
        while(true)
        {
            if (events.Count > 0)
            {
                for(int i = 0; i < events.Count; ++i)
                {
                    if (events[i].Condition())
                    {
                        events[i].Action();
                        RemoveEvent(events[i]);
                        --i;
                    }
                }
            }

            yield return null;
        }
    }

    // 이벤트 삽입
    public void AddEvent(Event _event) 
    { 
        events.Add(_event);
        _event.AddEventAction();

        if(checkCoroutine == null)
        {
            checkCoroutine = CheckCoroutine();
            StartCoroutine(checkCoroutine);
        }
    }
    

    // 이벤트 삭제 
    public void RemoveEvent(Event _event)
    {
        events.Remove(_event);

        if(events.Count == 0)
        {
            StopCoroutine(checkCoroutine);
            checkCoroutine = null;
        }
    }
}