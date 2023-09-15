public class CuttingBoard : Cookware
{
    public override bool Interact()
    {
        if (base.Interact())
        {
            LinkManager.Instance.GetLinkedObject(this).StartChopAnimation();
        }
        return true;
    }

    protected override void CompletedCook()
    {
        LinkManager.Instance.GetLinkedObject(this).StopChopAnimation();
    }

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
