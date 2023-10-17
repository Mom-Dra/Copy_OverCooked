public class FixedContainer : Container
{
    protected override void ThrowPut(InteractableObject interactableObject)
    {
        if (!HasObject())
        {
            Put(interactableObject);
        }
    }
}
