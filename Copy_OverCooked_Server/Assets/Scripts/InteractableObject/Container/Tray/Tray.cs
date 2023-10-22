using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tray : Container, IFoodUIAttachable, IStateUIAttachable
{
    [Header("Tray")]
    [SerializeField]
    protected Vector3 stateUIOffset = Vector3.down;
    [SerializeField]
    protected bool PlusBaseUI = true;

    //[SerializeField]
    //protected List<EObjectSerialCode> ingredients = new List<EObjectSerialCode>();

    protected FoodUIComponent uIComponent;

    protected Image stateImage;

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

    public Image StateUI 
    {
        get => stateImage;
        set
        {
            stateImage = value;
            if(stateImage != null)
            {
                stateImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + stateUIOffset;
            }
        }
    }

    protected override void Awake()
    {
        if (PlusBaseUI)
        {
            uIComponent = new BaseUIComponent(transform, uIOffset, maxContainCount);
        } 
        else
        {
            uIComponent = new FoodUIComponent(transform, uIOffset);
        }
        base.Awake();
    }

    protected virtual void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + displayOffset;
        }

        if (uIComponent != null && (PlusBaseUI || uIComponent.HasImage))
        {
            uIComponent.OnImagePositionUpdate();
        }

        if(stateImage != null)
        {
            stateImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + stateUIOffset;
        }
    }

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
            if (putIFood.FoodUIComponent.HasImage)
            {
                putIFood.FoodUIComponent.Clear();
            }

            base.Put(interactableObject);
            uIComponent.AddRange(putIFood.Ingredients);
        }
    }

    protected bool TryCheckRecipe(ECookingMethod cookingMethod, IFood iFood, out Recipe recipe)
    {
        recipe = null;
        List<EObjectSerialCode> ingredients = new List<EObjectSerialCode>();
        ingredients.AddRange(iFood.Ingredients);
        if (TryGet<FoodTray>(out FoodTray foodTray))
        {
            ingredients.Add(foodTray.SerialCode);
        } else if(HasObject())
        {
            ingredients.AddRange(Ingredients);
        }

        RecipeManager.Instance.TryGetRecipe(cookingMethod, ingredients, out recipe);
        return recipe != null;
    }

    public override void Remove()
    {
        base.Remove();
        //ingredients.Clear();
        uIComponent.Clear();
    }

    public void AddIngredientImages()
    {
        uIComponent.AddRange(Ingredients);
    }

    protected override bool CanGet()
    {
        if(getObject.TryGet<Food>(out Food food))
        {
            return food.FoodState != EFoodState.Burned;
        }
        return true;
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
