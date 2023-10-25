using UnityEngine;

public class Mixer : Cookware
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryGet<Food>(out Food mixed))
            {
                currTotalCookDuration = 5 + mixed.Ingredients.Count * 2f;
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

    public override void Remove()
    {
        base.Remove();
        currTotalCookDuration = totalCookDuration;
    }
}