public class Trashcan : FixedContainer
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        Destroy(interactableObject.gameObject);
        return true;
    }
}