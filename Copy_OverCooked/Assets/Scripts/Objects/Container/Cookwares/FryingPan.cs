public class FryingPan : Cookware
{
    private bool OnInduction = false;

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
