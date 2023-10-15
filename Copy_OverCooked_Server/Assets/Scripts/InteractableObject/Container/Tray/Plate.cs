public class Plate : Tray
{
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (base.IsValidObject(interactableObject) && interactableObject.TryFind<Food>(out Food food))
        {
            return food.FoodState == EFoodState.Cooked;
        }
        return false;
    }
}