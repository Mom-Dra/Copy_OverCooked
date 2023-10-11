public class Plate : Tray
{
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (interactableObject.TryGet<Food>(out Food food) && food.FoodState != EFoodState.Original)
        {
            return base.IsValidObject(interactableObject);
        }
        return false;
    }
}