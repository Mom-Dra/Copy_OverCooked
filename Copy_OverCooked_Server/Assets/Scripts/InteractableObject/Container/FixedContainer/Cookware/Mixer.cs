using UnityEngine;

public class Mixer : Cookware
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryGet<Food>(out Food mixed))
            {
                if (cookwareState == ECookwareState.Idle)
                {
                    selectedCoroutine = CookCoroutine(mixed);
                    StartCoroutine(selectedCoroutine);
                }
                else if(cookwareState == ECookwareState.Cook && interactableObject.TryGetComponent<IFood>(out IFood iFood))
                {
                    // 조리 시간 조정 
                    mixed.CurrCookingRate = (mixed.CurrCookingRate < iFood.CurrCookingRate) ? mixed.CurrCookingRate : iFood.CurrCookingRate;
                }
                else if (cookwareState == ECookwareState.Complete)
                {
                    StopSelectedCoroutine();
                    if(StateUIAttachable.StateUI != null)
                    {
                        Destroy(StateUIAttachable.StateUI.gameObject);
                        StateUIAttachable.StateUI = null;
                    }
                    StartCoroutine(CookCoroutine(mixed));
                }
            }
            return true;
        }
        return false;
    }

    protected override bool CanCook()
    {
        return true;
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<MixerTray>(out MixerTray tray);
    }

    public override void OnProgressBegin()
    {

    }

    public override void OnProgressEnd()
    {

    }
}