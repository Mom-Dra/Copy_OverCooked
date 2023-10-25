using System.Collections.Generic;
using UnityEngine;

public class Induction : Cookware
{
    private Renderer burnerRenderer;
    private static Color originBurnerColor;

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
            if (TryGet<Food>(out Food food))
            {
                TryCook();
            }
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
        return true;
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