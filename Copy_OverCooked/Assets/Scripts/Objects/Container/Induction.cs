using Unity.VisualScripting;
using UnityEngine;

public class Induction : Container
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        Debug.Log("Induction TryPut11");
        if (base.TryPut(interactableObject))
        {
            Debug.Log("Induction TryPut22");
            if (getObject != null)
            {
                Cookware cookware = getObject as Cookware;
                cookware.TryCook();
            }
            return true;
        }
        return false;
    }

    public override void Fit(InteractableObject interactableObject)
    {
        interactableObject.transform.position = transform.position + containOffset;
        interactableObject.Fix();
    }

    public override bool IsValidObject(InteractableObject interactableObject)
    {
        return true;
    }
}