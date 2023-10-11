using UnityEngine;

public class UIStateEvent : Event
{
    private Cookware cookware;

    public UIStateEvent(Cookware container)
    {
        this.cookware = container;
        actions.Add(ShowCompleteUI);
        if (container.Flammablity)
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
            int currDegree = food.CurrCookDegree;
            if (currDegree >= 100 && currDegree < 160)
            {
                if (cookware.UIImage != null)
                {
                    GameObject.Destroy(cookware.UIImage.gameObject);
                }
                cookware.UIImage = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Complete);
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
            int currDegree = food.CurrCookDegree;
            if (currDegree >= 160 && currDegree < 200)
            {
                if (cookware.UIImage != null)
                {
                    GameObject.Destroy(cookware.UIImage.gameObject);
                }
                cookware.UIImage = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Warning);
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
            int currDegree = food.CurrCookDegree;
            if (currDegree >= 200)
            {
                if (cookware.UIImage != null)
                {
                    GameObject.Destroy(cookware.UIImage.gameObject);
                }
                cookware.UIImage = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Overheat);
                cookware.CookwareState = ECookwareState.Overheat;
                return true;
            }
        }
        return false;
    }

    public override void AddEventAction()
    {

    }
}