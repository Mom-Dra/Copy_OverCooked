public class FryingPan : Tray
{
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return base.IsValidObject(interactableObject) && interactableObject.TryGetComponent<Food>(out Food value);
    }
}