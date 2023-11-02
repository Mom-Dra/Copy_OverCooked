using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tray : Container
{
    [Header("Tray")]
    protected FoodUIComponent uIComponent;

    public virtual List<EObjectSerialCode> Ingredients
    {
        get
        {
            return getObject?.GetComponent<IFood>()?.Ingredients;
        }
    }

    public FoodUIComponent FoodUIComponent
    {
        get => uIComponent;
    }

    //private void FixedUpdate()
    //{
    //    if (getObject != null)
    //    {
    //        getObject.transform.position = transform.position + displayOffset;
    //        getObject.transform.rotation = transform.rotation;
    //    }
    //}

    public override bool TryPut(InteractableObject interactableObject)
    {
        if (IsValidObject(interactableObject))
        {
            Put(interactableObject);
            return true;
        }
        return false;
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return interactableObject.TryGetComponent<IFood>(out IFood iFood) && iFood.FoodState != EFoodState.Burned;
    }

    public override void Put(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<IFood>(out IFood putIFood))
        {
            putIFood.FoodUIComponent.Clear();
            
            base.Put(interactableObject);

            uIComponent.AddRange(putIFood.Ingredients);
        }
    }

    public override void Remove(InteractableObject interactableObject)
    {
        base.Remove(interactableObject);
        uIComponent.Clear();
    }

    public void AddIngredientImages()
    {
        uIComponent.AddRange(Ingredients);
    }

    protected virtual void OnDestroy()
    {
        if (HasObject())
        {
            Destroy(getObject.gameObject);
            getObject = null;
        }
    }
}
