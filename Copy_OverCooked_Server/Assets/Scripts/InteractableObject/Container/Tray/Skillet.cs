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

    // 추후 이거 코드 중복이므로 수정 바람 
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (base.IsValidObject(interactableObject) && interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            return TryCheckRecipe(cookingMethod, iFood, out Recipe recipe);
        }
        return false;
    }
}