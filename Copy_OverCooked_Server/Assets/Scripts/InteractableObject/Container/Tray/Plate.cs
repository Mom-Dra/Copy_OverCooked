using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Plate : Tray
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
        cleanColor = plateRenderer.material.color;
    }

    private void OnPlateStateChanging(EPlateState plateState)
    {
        if(plateState == EPlateState.Clean)
        {
            plateRenderer.material.color = cleanColor;
        } 
        else
        {
            plateRenderer.material.color = Color.gray;
        }
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if(plateState == EPlateState.Clean)
        {
            if (base.IsValidObject(interactableObject) && interactableObject.TryGetComponent<IFood>(out IFood iFood))
            {
                if (!iFood.IsCookable || iFood.FoodState == EFoodState.Cooked)
                {
                    return !HasObject() || TryCheckRecipe(ECookingMethod.Combine, iFood, out Recipe recipe);
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
                } 
                else
                {
                    PutAndCombine(iFood);
                }
            }
        } 
        else
        {
            Stack(interactableObject);
        }
    }

    public void Stack(InteractableObject interactableObject)
    {
        if (getObject != null && getObject.TryGet<Plate>(out Plate plate))
        {
            plate.Stack(interactableObject);
        } else
        {
            GetObject = interactableObject;
        }
    }

    public override void Remove()
    {
        base.Remove();
    }
}