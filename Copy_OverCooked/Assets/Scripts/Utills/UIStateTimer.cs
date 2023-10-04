using System.Collections;
using UnityEngine;

public class UIStateTimer
{
    Container container;

    public UIStateTimer(Container container)
    {
        this.container = container;
    }

    public IEnumerator GetCoroutine(int currDegree)
    {
        switch (currDegree)
        {
            case 100:
                return CompleteCoroutine();
            case 150:
                return WarningCoroutine();
            case 200:
                return OverheatCoroutine();
        }
        return null;
    }

    private IEnumerator CompleteCoroutine()
    {
        container.uIImage = InstantiateManager.Instance.InstantiateUI(container, EInGameUIType.Complete);
        while (true)
        {
            yield return null;
        }
    }

    private IEnumerator WarningCoroutine()
    {
        container.uIImage = InstantiateManager.Instance.InstantiateUI(container, EInGameUIType.Warning);
        while (true)
        {
            yield return null;
        }
    }

    private IEnumerator OverheatCoroutine()
    {
        container.uIImage = InstantiateManager.Instance.InstantiateUI(container, EInGameUIType.Overheat);
        while (true)
        {
            yield return null;
        }
    }
    
}