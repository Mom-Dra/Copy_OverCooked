using UnityEngine;

public class CuttingBoard : Cookware
{
    public override void Fit(InteractableObject interactableObject)
    {
        interactableObject.transform.position = transform.position + offset;
        interactableObject.FixFromExternalPhysics();
    }

    public override bool IsValidObject(InteractableObject gameObject)
    {
        return true;
    }
}
