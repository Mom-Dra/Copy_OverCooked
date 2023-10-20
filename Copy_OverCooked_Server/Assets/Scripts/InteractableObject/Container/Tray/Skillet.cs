using System.Collections.Generic;
using UnityEngine;

public class Skillet : Tray, ICookableTray
{
    [SerializeField]
    private ECookingMethod cookingMethod;
    public ECookingMethod CookingMethod
    {
        get { return cookingMethod; }
    }

    // ���� �̰� �ڵ� �ߺ��̹Ƿ� ���� �ٶ� 
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (base.IsValidObject(interactableObject) && interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            return TryCheckRecipe(cookingMethod, iFood, out Recipe recipe);
        }
        return false;
    }
}