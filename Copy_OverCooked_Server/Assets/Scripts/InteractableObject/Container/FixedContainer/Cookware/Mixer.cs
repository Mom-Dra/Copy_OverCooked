using UnityEngine;

public class Mixer : Cookware
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryGet<Mixed>(out Mixed mixed))
            {
                currTotalCookDuration = 7 + mixed.Ingredients.Count * 4f;
                if (cookwareState == ECookwareState.Idle)
                {
                    selectedCoroutine = CookCoroutine(mixed, false);
                    StartCoroutine(selectedCoroutine);
                } 
                else if(interactableObject.TryGetComponent<IFood>(out IFood iFood))
                {
                    if (cookwareState == ECookwareState.Complete)
                    {
                        StopSelectedCoroutine();
                        if (StateUIAttachable.StateUI != null)
                        {
                            Destroy(StateUIAttachable.StateUI.gameObject);
                            StateUIAttachable.StateUI = null;
                        }
                        selectedCoroutine = CookCoroutine(mixed, false);
                        StartCoroutine(selectedCoroutine);
                    }
                }
                
            }
            return true;
        }
        return false;
    }

    protected override bool CanCook()
    {
        return TryGet<MixerTray>(out MixerTray tray);
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<MixerTray>(out MixerTray tray);
    }

    protected override void ThrowPut(InteractableObject interactableObject)
    {
        if (!TryCook())
        {
            base.ThrowPut(interactableObject);
        }
    }

    public override void OnProgressBegin()
    {

    }

    public override void OnProgressEnd()
    {

    }

    public override void Remove(InteractableObject interactableObject)
    {
        base.Remove(interactableObject);
        currTotalCookDuration = totalCookDuration;
    }
}