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
        if (!cookware.HasObject())
            return true;
        if (cookware.TryFind<Food>(out Food food))
        {
            if (food.CurrOverTime > 0)
            {
                RemoveStateImage();
                cookware.StateImage = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Completed);
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
                RemoveStateImage();
                cookware.StateImage = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Warning);
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
                RemoveStateImage();
                cookware.StateImage = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Overheat);
                cookware.OnOverheat();
                return true;
            }
        }
        return false;
    }

    private void RemoveStateImage()
    {
        if (cookware.StateImage != null)
        {
            GameObject.Destroy(cookware.StateImage.gameObject);
        }
    }

    public override void AddEventAction()
    {

    }
}