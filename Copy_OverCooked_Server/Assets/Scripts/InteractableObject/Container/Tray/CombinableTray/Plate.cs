using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Plate : CombinableTray
{
    [SerializeField]
    private EPlateState plateState = EPlateState.Clean;

    private Renderer plateRenderer;
    private Color cleanColor;

    public EPlateState PlateState
    {
        get 
        { 
            return plateState; 
        }
        set
        {
            if(plateState != value)
            {
                OnPlateStateChanging(value);
            }
            plateState = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        plateRenderer = GetComponent<Renderer>();
        if(plateRenderer != null )
        { 
            cleanColor = plateRenderer.material.color;
        }
    }

    private void OnPlateStateChanging(EPlateState plateState)
    {
        //if(plateState == EPlateState.Clean)
        //{
        //    plateRenderer.material.color = cleanColor;
        //} 
        //else
        //{
        //    plateRenderer.material.color = Color.gray;
        //}
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if(plateState == EPlateState.Clean)
        {
            if (interactableObject.TryGetComponent<IFood>(out IFood iFood) && iFood.CookingMethod != ECookingMethod.Mix)
            {
                if (!iFood.IsCookable || iFood.FoodState == EFoodState.Cooked)
                {
                    return base.IsValidObject(interactableObject);
                    //return !HasObject() || TryCheckRecipe(ECookingMethod.Combine, iFood, out Recipe recipe);
                }
            }
            return false;
        }
        else return interactableObject.TryGet<Plate>(out Plate plate) && plate.PlateState == EPlateState.Dirty;
    }

    public override void Put(InteractableObject interactableObject)
    {
        if(plateState == EPlateState.Clean)
        {
            if(interactableObject.TryGetComponent(out IFood iFood))
            {
                if (!HasObject())
                {
                    base.Put(interactableObject);
                    iFood.OnPlated();
                } 
                else
                {
                    PutDish(iFood);
                }
            }
        } 
        else
        {
            StackUp(interactableObject);
        }
    }

    public void StackUp(InteractableObject interactableObject)
    {
        if (getObject != null && getObject.TryGet<Plate>(out Plate plate))
        {
            plate.StackUp(interactableObject);
        } else
        {
            GetObject = interactableObject;
        }
    }

    public void RemoveSelf()
    {
        getObject = null;
        uIComponent.Clear();
    }
}