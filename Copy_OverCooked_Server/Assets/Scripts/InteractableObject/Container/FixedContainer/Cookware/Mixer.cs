using UnityEngine;

public class Mixer : Cookware
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryGet<Mixed>(out Mixed mixed))
            {
                currTotalCookDuration = totalCookDuration + mixed.Ingredients.Count * 4f;
                if (cookwareState == ECookwareState.Idle)
                {
                    selectedCoroutine = CookCoroutine(mixed);
                    StartCoroutine(selectedCoroutine);
                } 
                else if(interactableObject.TryGetComponent<IFood>(out IFood iFood))
                {
                    if (cookwareState == ECookwareState.Complete)
                    {
                        StopSelectedCoroutine();
                        if (ProgressUIAttachable.ProgressImage != null)
                        {
                            Destroy(ProgressUIAttachable.ProgressImage.gameObject);
                            ProgressUIAttachable.ProgressImage = null;
                        }
                        selectedCoroutine = CookCoroutine(mixed);
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

    public override void OnProgressBegin()
    {

    }

    public override void OnProgressEnd()
    {

    }

    public override void Remove(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent<CookableTray>(out CookableTray cookableTray))
        {
            cookableTray.ParentCookware = null;
        }
        base.Remove(interactableObject);
        currTotalCookDuration = totalCookDuration;
    }
}