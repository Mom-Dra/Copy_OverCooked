public class FryingPan : Tray
{
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (base.IsValidObject(interactableObject) && interactableObject.TryFind<Food>(out Food food))
        {
            return true;
        }
        return false;
    }
}