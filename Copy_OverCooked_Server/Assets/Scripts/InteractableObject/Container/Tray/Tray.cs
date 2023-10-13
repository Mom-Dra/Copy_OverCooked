using UnityEngine;
using UnityEngine.UI;

public class Tray : Container
{
    [Header("Tray")]
    [SerializeField]
    private bool PlusBaseUI = true;

    private UIComponent baseUIComponent;

    public override Image Image
    {
        set
        {
            base.Image = value;
            if (PlusBaseUI)
            {
                if (value != null)
                {
                    if (baseUIComponent.Count == 1)
                    {
                        for (int i = 0; i < maxContainCount - 1; ++i)
                        {
                            baseUIComponent.Add(InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.PlusBase));
                        }
                    }
                } 
                else
                {
                    baseUIComponent.DestroyAllImages();
                    baseUIComponent.Add(InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.PlusBase));
                }
            }
        }
    }

    private void Awake()
    { 
        if(PlusBaseUI)
        {
            uIComponent.SetOffsetIndex(maxContainCount);
            baseUIComponent = new UIComponent();
            baseUIComponent.Add(InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.PlusBase));
        }
    }

    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + displayOffset;
        }
        if (uIComponent.Count > 0)
        {
            uIComponent.OnUIPositionChanging(transform, uIOffset);
        }
        if (baseUIComponent.Count > 0)
        {
            baseUIComponent.OnUIPositionChanging(transform, uIOffset);
        }
    }

    public override bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek)
    {
        if(base.TryGet(out result, getMode)){
            if(getMode == EGetMode.Pop && uIComponent.Count > 0)
            {
                foreach (Image image in uIComponent.Images)
                {
                    getObject.Image = image;
                }
            }
            return true;
        }
        return false;
    }

    protected override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
        if(interactableObject.UIComponent.Count > 0)
        {
            foreach(Image image in interactableObject.UIComponent.Images)
            {
                Image = image;
            }
        }
    }

    public override void Remove(InteractableObject interactableObject)
    {
        base.Remove(interactableObject);
        if(uIComponent.Count > 0)
        {
            Image = null;
        }
    }
}
