using System.Collections.Generic;

public class Skillet : Tray
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