using System.Collections.Generic;
using UnityEngine;

public class Induction : Cookware
{
    private Renderer burnerRenderer;
    private static Color originBurnerColor;

    protected override ECookingMethod CookingMethod
    {
        get => getObject.GetComponent<CookableTray>().CookingMethod;
    }

    protected override void Awake()
    {
        base.Awake();
        burnerRenderer = transform.Find("Burner").GetComponent<Renderer>();
        originBurnerColor = burnerRenderer.material.color;
    }

    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            TryCook();
            return true;
        }
        return false;
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<Tray>(out Tray tray);
    }

    protected override bool CanCook()
    {
        return TryGet<CookableTray>(out CookableTray tray);
    }

    protected override void ThrowPut(InteractableObject interactableObject)
    {
        if (!TryCook())
        {
            base.ThrowPut(interactableObject);
        }
    }

    public override void OnProgressBegin()
    {
        burnerRenderer.material.color = Color.red;
    }

    public override void OnProgressEnd()
    {
        burnerRenderer.material.color = originBurnerColor;
    }
}