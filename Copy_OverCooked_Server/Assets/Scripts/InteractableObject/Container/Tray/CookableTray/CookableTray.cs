using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CookableTray : Tray, IProgressUIAttachable
{
    [Header("Cookable Tray")]
    [SerializeField]
    protected int maxContainCount = 1;
    [SerializeField]
    private ECookingMethod cookingMethod;
    [SerializeField]
    protected Vector3 stateImageOffset = new Vector3(0f, -75f, 0f);

    protected Cookware parentCookware;

    protected Image progressImage;

    public ECookingMethod CookingMethod
    {
        get => cookingMethod; 
    }

    public Cookware ParentCookware
    {
        set
        {
            parentCookware = value;
        }
    }



    public Image ProgressImage
    {
        get => progressImage;
        set
        {
            progressImage = value;
            if (progressImage != null)
            {
                progressImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + stateImageOffset;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        uIComponent = new BaseUIComponent(transform, uIOffset, maxContainCount);
    }

    private void FixedUpdate()
    {
        if (uIComponent != null)
        {
            uIComponent.OnImagePositionUpdate();
        }

        if(progressImage != null)
        {
            progressImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + stateImageOffset;
        }
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (!HasObject() && base.IsValidObject(interactableObject) && interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            if(iFood.FoodState == EFoodState.Cooking && iFood.CookingMethod != cookingMethod)
            {
                return false;
            }
            return TryCheckRecipe(cookingMethod, iFood, out Recipe recipe) || RecipeManager.Instance.FindCookedFood(cookingMethod, interactableObject.SerialCode);
        }
        return false;
    }

    protected bool TryCheckRecipe(ECookingMethod cookingMethod, IFood iFood, out Recipe recipe)
    {
        recipe = null;
        List<EObjectSerialCode> ingredients = new List<EObjectSerialCode>();
        ingredients.AddRange(iFood.Ingredients);
        if (TryGet<FoodTray>(out FoodTray foodTray))
        {
            ingredients.Add(foodTray.SerialCode);
        } else if (HasObject())
        {
            ingredients.AddRange(Ingredients);
        }

        RecipeManager.Instance.TryGetRecipe(cookingMethod, ingredients, out recipe);
        return recipe != null;
    }

    public override void Remove(InteractableObject interactableObject)
    {
        base.Remove(interactableObject);
        if(progressImage != null)
        {
            Destroy(progressImage.gameObject);
            progressImage = null;
        }
    }

    protected override void ThrowPut(InteractableObject interactableObject)
    {
        parentCookware.TryPut(interactableObject);
    }

    public abstract void OnProgressBegin();

    public abstract void OnProgressEnd();
}