using UnityEngine;

public class Oven : Cookware
{
    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryGet<IFood>(out IFood food))
            {
                if (TryCook())
                {
                    if (!FoodUIAttachable.FoodUIComponent.HasImage)
                    {
                        FoodUIAttachable.AddIngredientImages();
                    }
                }
            }
            return true;
        }
        return false;
    }

    protected override bool CanCook()
    {
        return TryGet<IFood>(out IFood tray);
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<Tray>(out Tray tray)
            || RecipeManager.Instance.FindCookedFood(cookingMethod, interactableObject.SerialCode);
    }

    public override void Remove(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent<CookableTray>(out CookableTray cookableTray))
        {
            cookableTray.ParentCookware = null;
        }
        base.Remove(interactableObject);
    }

    public override void OnProgressBegin()
    {

    }

    public override void OnProgressEnd()
    {

    }
}