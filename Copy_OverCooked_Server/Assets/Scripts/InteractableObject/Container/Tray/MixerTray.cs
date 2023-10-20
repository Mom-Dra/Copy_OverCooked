using UnityEngine;

public class MixerTray : Tray
{
    // 추후 이거 코드 중복이므로 수정 바람 
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (base.IsValidObject(interactableObject) && interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            return TryCheckRecipe(ECookingMethod.Mix, iFood, out Recipe recipe);
        }
        return false;
    }
}