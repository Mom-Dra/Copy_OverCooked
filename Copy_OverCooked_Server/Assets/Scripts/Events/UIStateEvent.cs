using UnityEngine;
using UnityEngine.UI;

public class UIStateEvent : Event
{
    private Cookware cookware;

    private Image showImage;

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
                showImage = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Img_Completed).GetComponent<Image>();
                cookware.StateImage = showImage.InstantiateOnCanvas();
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
                showImage = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Img_Warning).GetComponent<Image>();
                cookware.StateImage = showImage.InstantiateOnCanvas();
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
                showImage = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Img_Overheat).GetComponent<Image>();
                cookware.StateImage = showImage.InstantiateOnCanvas();
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