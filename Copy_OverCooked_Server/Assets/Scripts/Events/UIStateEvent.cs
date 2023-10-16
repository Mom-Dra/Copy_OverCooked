using UnityEngine;

public class UIStateEvent : Event
{
    private Cookware cookware;

    public UIStateEvent(Cookware cookware)
    {
        this.cookware = cookware;
        this.cookware.UIComponent.Clear();

        if (cookware.Flammablity)
        {
            actions.Add(ShowCompleteUI);
            actions.Add(ShowWarningUI);
            actions.Add(ShowOverheatUI);
        }
    }

    private bool ShowCompleteUI()
    {
        if (!cookware.HasObject())
            return true;
        if (cookware.TryFind<Food>(out Food food))
        {
            if (food.CurrOverTime > 0)
            {
                cookware.UIComponent.Add(EObjectSerialCode.Img_Completed);
                return true;
            }
        }
        return false;
    }

    private bool ShowWarningUI()
    {
        
        if (!cookware.HasObject())
            return true;
        if (cookware.TryFind<Food>(out Food food))
        {
            if (food.CurrOverTime >= 60)
            {
                if (cookware.UIComponent.HasImage)
                {
                    cookware.UIComponent.Clear();
                }
                cookware.UIComponent.Add(EObjectSerialCode.Img_Warning);
                return true;
            }
        }
        return false;
    }

    private bool ShowOverheatUI()
    {
        if (!cookware.HasObject())
            return true;
        if (cookware.TryFind<Food>(out Food food))
        {
            if (food.CurrOverTime >= 100)
            {
                if (cookware.UIComponent.HasImage)
                {
                    cookware.UIComponent.Clear();
                }
                if(cookware.GetObject.TryFind<Tray>(out Tray tray))
                {
                    tray.UIComponent.Clear();
                    tray.UIComponent.Add(EObjectSerialCode.Img_Overheat);
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