public class Trashcan : Container
{
    protected override bool Put(InteractableObject interactableObject)
    {
        interactableObject?.gameObject.DebugName("Trash!", EDebugColor.Red);
        Destroy(interactableObject.gameObject);
        return true;
    }

    public override bool IsValidObject(InteractableObject gameObject)
    {
        return true;
    }
}
