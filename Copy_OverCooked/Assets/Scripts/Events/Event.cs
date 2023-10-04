using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Event
{
    protected List<Func<bool>> actions = new List<Func<bool>>();
    protected int currIndex = 0;
    // CurrAction ������ ���� 
    // EventAction �� �迭�� �����,
    // �� Event���� 

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
        // ���� �׼��� �����ϰ�,
        // �����Ͽ��ٸ� ���� �׼��� ���� �׼����� ��� 
    }
    public abstract void AddEventAction(); 
}