using UnityEngine;

public class Tray : Container
{
    [Header("Tray")]
    [SerializeField]
    private bool PlusBaseUI = true;

    private UIComponent baseUIComponent;

    protected override void Awake()
    {
        base.Awake();
        if(PlusBaseUI)
        {
            baseUIComponent = new UIComponent(transform, uIOffset);
            for(int i = 0; i< maxContainCount; i++)
            {
                baseUIComponent.Image = InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.PlusBase);
            }
        }
    }

    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + displayOffset;
        }
        if (uIComponent.Index > 0)
        {
            uIComponent.OnUIPositionChanging();
        }
        //if (baseUIComponent.Index > 0)
        //{
        //    baseUIComponent.OnUIPositionChanging();
        //}
    }
}
