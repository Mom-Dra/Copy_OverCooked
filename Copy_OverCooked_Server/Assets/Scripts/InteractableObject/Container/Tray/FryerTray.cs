using UnityEngine;

public class FryerTray : Tray
{
    // ���� �̰� �ڵ� �ߺ��̹Ƿ� ���� �ٶ� 
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (base.IsValidObject(interactableObject) && interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            return TryCheckRecipe(ECookingMethod.Fry, iFood, out Recipe recipe);
        }
        return false;
    }
}