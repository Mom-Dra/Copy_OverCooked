using UnityEngine;

public class MixerTray : Tray
{
    // ���� �̰� �ڵ� �ߺ��̹Ƿ� ���� �ٶ� 
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (base.IsValidObject(interactableObject) && interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            return TryCheckRecipe(ECookingMethod.Mix, iFood, out Recipe recipe);
        }
        return false;
    }
}