using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // EventManager = Event 관리 
    // 1. 이벤트 멤버들
    // 2. 이벤트 조건
    // 3. 이벤트 행동 
    private static EventManager instance;
    public static EventManager Instance
    {
        get => instance;
    }

    private List<Event> events;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            events = new List<Event>();
            DontDestroyOnLoad(this);
        } 
        else
        {
            Destroy(this);
        }
    }

    // 이벤트 조건절 검사 
    private void Update()
    {
        if (events.Count > 0)
        {
            foreach (Event e in events) // 요기서 InvalidOperation 오류뜸, 배열이 변경되었다고 뜨는데
            {
                if (e.Condition())
                {
                    e.Action();
                    RemoveEvent(e);
                }
            }
        }
    }

    // 이벤트 삽입
    public void AddEvent(Event _event) 
    { 
        events.Add(_event);
    }
    

    // 이벤트 삭제 
    public void RemoveEvent(Event _event)
    {
        events.Remove(_event);
    }
}