using UnityEngine;

public class UIStateEvent : Event
{
    Cookware cookware;

    public UIStateEvent(Cookware container)
    {
        this.cookware = container;
        actions.Add(ShowCompleteUI);
        if (container.IsFirable)
        {
            actions.Add(ShowWarningUI);
            actions.Add(ShowOverheatUI);
        }
    }

    private bool ShowCompleteUI()
    {
        if (!cookware.HasObject())
            return true;
        if (cookware.TryGet<Food>(out Food food))
        {
            int currDegree = food.currCookDegree;
            if (currDegree >= 100 && currDegree < 160)
            {
                if (cookware.uIImage != null)
                {
                    GameObject.Destroy(cookware.uIImage.gameObject);
                }
                cookware.uIImage = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Complete);
                return true;
            }
        }
        return false;
    }

    private bool ShowWarningUI()
    {
        if (!cookware.HasObject())
            return true;
        if (cookware.TryGet<Food>(out Food food))
        {
            int currDegree = food.currCookDegree;
            if (currDegree >= 160 && currDegree < 200)
            {
                if (cookware.uIImage != null)
                {
                    GameObject.Destroy(cookware.uIImage.gameObject);
                }
                cookware.uIImage = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Warning);
                return true;
            }
        }
        return false;
    }

    private bool ShowOverheatUI()
    {
        if (!cookware.HasObject())
            return true;
        if (cookware.TryGet<Food>(out Food food))
        {
            int currDegree = food.currCookDegree;
            if (currDegree >= 200)
            {
                if (cookware.uIImage != null)
                {
                    GameObject.Destroy(cookware.uIImage.gameObject);
                }
                cookware.uIImage = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Overheat);
                cookware.cookwareState = ECookwareState.Overheat;
                return true;
            }
        }
        return false;
    }

    public override void AddEventAction()
    {

    }
}