using UnityEngine;

public class Table : Container
{
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
