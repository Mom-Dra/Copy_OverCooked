using System.Collections;
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

    private IEnumerator checkCoroutine;
    private bool isPlaying;

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
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(this);
        }
    }

    private IEnumerator CheckCoroutine()
    {
        while(true)
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
            } else break;

            yield return null;
        }
    }

    // �̺�Ʈ ����
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
    

    // �̺�Ʈ ���� 
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