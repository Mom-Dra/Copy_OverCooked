using System.Collections.Generic;

public class Plate : Tray
{
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (base.IsValidObject(interactableObject))
        {
            return true;
        }
        return false;
    }
}