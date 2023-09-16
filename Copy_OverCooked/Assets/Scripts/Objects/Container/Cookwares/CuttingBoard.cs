using UnityEngine;

public class CuttingBoard : Cookware
{
    public override bool Interact()
    {
        if (base.Interact())
        {
            LinkManager.Instance.GetLinkedPlayer(this).SetBoolAnimation(EAnimationType.Chop, true);
        }
        return true;
    }

    protected override void CompletedCook()
    {
        LinkManager.Instance.GetLinkedPlayer(this).SetBoolAnimation(EAnimationType.Chop, false);
    }

    public override void Fit(InteractableObject interactableObject)
    {
        Debug.Log("<color=orange> Fit </color>");
        interactableObject.Fix();
        interactableObject.transform.position = transform.position + offset;
    }

    public override bool IsValidObject(InteractableObject interactableObject)
    {
        return interactableObject.GetObjectType() == EObjectType.Food;
    }
}
