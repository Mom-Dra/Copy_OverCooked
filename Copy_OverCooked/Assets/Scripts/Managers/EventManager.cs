using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // EventManager = Event ���� 
    // 1. �̺�Ʈ �����
    // 2. �̺�Ʈ ����
    // 3. �̺�Ʈ �ൿ 
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

    // �̺�Ʈ ������ �˻� 
    private void Update()
    {
        if (events.Count > 0)
        {
            foreach (Event e in events) // ��⼭ InvalidOperation ������, �迭�� ����Ǿ��ٰ� �ߴµ�
            {
                if (e.Condition())
                {
                    e.Action();
                    RemoveEvent(e);
                }
            }
        }
    }

    // �̺�Ʈ ����
    public void AddEvent(Event _event) 
    { 
        events.Add(_event);
    }
    

    // �̺�Ʈ ���� 
    public void RemoveEvent(Event _event)
    {
        events.Remove(_event);
    }
}