public class FryingPan : Cookware
{
    public override void Fit(InteractableObject interactableObject)
    {
        interactableObject.Fix();
        interactableObject.transform.position = transform.position + containOffset;
    }

    public override bool IsValidObject(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent<Food>(out Food food))
        {
            if (food.cookingMethod == ECookingMethod.Grill)
            {
                return true;
            }
        }
        return false;
    }
}
