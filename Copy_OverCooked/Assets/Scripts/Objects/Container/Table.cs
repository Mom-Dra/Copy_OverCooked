using UnityEngine;

public class Table : Container
{
    public override bool IsValidObject(InteractableObject interactableObject)
    {
        return true;
    }
}
