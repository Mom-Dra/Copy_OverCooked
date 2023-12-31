using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Plate : CombinableTray
{
    [Header("Plate")]
    [SerializeField]
    private EPlateState plateState = EPlateState.Clean;

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
        cleanColor = renderers[0].material.color;
    }

    private void OnPlateStateChanging(EPlateState plateState)
    {
        if (plateState == EPlateState.Clean)
        {
            SetColorInRenderers(cleanColor);
        } 
        else
        {
            SetColorInRenderers(Color.gray);
        }
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
                    CombineToDish(iFood);
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