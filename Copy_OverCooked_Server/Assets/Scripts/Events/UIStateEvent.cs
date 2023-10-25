using UnityEngine;
using UnityEngine.UI;

public class UIStateEvent : Event
{
    private Cookware cookware;

    public UIStateEvent(Cookware cookware)
    {
        this.cookware = cookware;

        if (cookware.Flammablity)
        {
            actions.Add(ShowCompleteUI);
            actions.Add(ShowWarningUI);
            actions.Add(ShowOverheatUI);
        }
    }

    private bool ShowCompleteUI()
    {
        if (IsDisableToEvent())
            return true;
        if (cookware.TryGet<Food>(out Food food))
        {
            if (food.CurrOverTime > 0)
            {
                RemoveStateImage();
                cookware.StateUIAttachable.StateUI = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Completed);
                return true;
            }
        }
        return false;
    }

    private bool ShowWarningUI()
    {
        if (IsDisableToEvent())
            return true;
        if (cookware.TryGet<Food>(out Food food))
        {
            if (food.CurrOverTime >= 60)
            {
                RemoveStateImage();
                cookware.StateUIAttachable.StateUI = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Warning);
                return true;
            }
        }
        return false;
    }

    private bool ShowOverheatUI()
    {
        if (IsDisableToEvent())
            return true;

        if (cookware.TryGet<Food>(out Food food))
        {
            if (food.CurrOverTime >= 100)
            {
                RemoveStateImage();
                cookware.OnOverheat();
                return true;
            }
        }
        return false;
    }

    private bool IsDisableToEvent()
    {
        return !cookware.HasObject() || cookware.CookwareState == ECookwareState.Cook;
    }

    private void RemoveStateImage()
    {
        if (cookware.StateUIAttachable.StateUI != null)
        {
            GameObject.Destroy(cookware.StateUIAttachable.StateUI.gameObject);
            cookware.StateUIAttachable.StateUI = null;
        }
    }

    public override void AddEventAction()
    {

    }
}