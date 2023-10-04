using UnityEngine;

public class UIStateEvent : Event
{
    Container container;

    public UIStateEvent(Container container)
    {
        this.container = container;
        actions.Add(ShowCompleteUI);
        if (container.IsFirable)
        {
            actions.Add(ShowWarningUI);
            actions.Add(ShowOverheatUI);
        }
    }

    private bool ShowCompleteUI()
    {
        int currDegree = container.getObject.GetComponent<Food>().currCookDegree;
        if(currDegree >= 100 && currDegree < 160)
        {
            if (container.uIImage != null)
            {
                GameObject.Destroy(container.uIImage.gameObject);
            }
            container.uIImage = InstantiateManager.Instance.InstantiateUI(container, EInGameUIType.Complete);
            return true;
        }
        return false;
    }

    private bool ShowWarningUI()
    {
        int currDegree = container.getObject.GetComponent<Food>().currCookDegree;
        if (currDegree >= 160 && currDegree < 200)
        {
            if (container.uIImage != null)
            {
                GameObject.Destroy(container.uIImage.gameObject);
            }
            container.uIImage = InstantiateManager.Instance.InstantiateUI(container, EInGameUIType.Warning);
            return true;
        }
        return false;
    }

    private bool ShowOverheatUI()
    {
        int currDegree = container.getObject.GetComponent<Food>().currCookDegree;
        if (currDegree >= 200)
        {
            if (container.uIImage != null)
            {
                GameObject.Destroy(container.uIImage.gameObject);
            }
            container.uIImage = InstantiateManager.Instance.InstantiateUI(container, EInGameUIType.Overheat);
            return true;
        }
        return false;
    }

    public override void AddEventAction()
    {

    }
}