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
            if (currDegree >= 100)
            {
                if (cookware.UIComponent.Count > 0)
                {
                    cookware.Image = null;
                }
                cookware.Image = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Complete);
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
            if (currDegree >= 160)
            {
                if (cookware.UIComponent.Count > 0)
                {
                    cookware.Image = null;
                }
                cookware.Image = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Warning);
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
                if (cookware.UIComponent.Count > 0)
                {
                    cookware.Image = null;
                }
                if(cookware.GetObject.TryGet<Tray>(out Tray tray))
                {
                    tray.Image = null;
                    tray.Image = InstantiateManager.Instance.InstantiateUI(cookware, EInGameUIType.Overheat);
                    cookware.CookwareState = ECookwareState.Overheat;
                } 
                else
                {
                    Debug.Log("UIStateEvent : Can't find tray in cookware");
                }
                return true;
            }
        }
        return false;
    }

    public override void AddEventAction()
    {

    }
}