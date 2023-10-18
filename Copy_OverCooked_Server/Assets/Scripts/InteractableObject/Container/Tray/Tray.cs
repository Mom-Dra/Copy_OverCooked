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
        if (interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            if (iFood.FoodState == EFoodState.Burned)
                return false;

            if (!HasObject())
                return true;

            return TryGetCombinedRecipe(iFood, out Recipe recipe);
        }
        return false;
    }

    public override void Put(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<IFood>(out IFood putIFood))
        {
            if (putIFood.FoodUIComponent.HasImage)
            {
                putIFood.FoodUIComponent.Clear();
            }

            if (!HasObject())
            {
                //base.Put(interactableObject);
                GetObject = interactableObject;
                uIComponent.AddRange(putIFood.Ingredients);
                return;
            }

            //// 여기까지 했음 
            //if (HasObject() && getObject.TryGetComponent<FoodTray>(out FoodTray putIFoodTray))
            //{
            //    putIFoodTray.Put(interactableObject);
            //    return;
            //}
            PutByRecipe(putIFood);
        }
    }

    protected void PutByRecipe(IFood iFood)
    {
        if (TryGetCombinedRecipe(iFood, out Recipe recipe))
        {
            // Plate만의 Prefab 까지 고려할 수 있게 코드 짜야함 
            Food combinedFood = Instantiate(SerialCodeDictionary.Instance.FindBySerialCode(recipe.CookedFood).GetComponent<Food>());

            if (HasObject() && !getObject.GetComponent<FoodTray>())
            {
                Destroy(getObject.gameObject);
                getObject = null;
            }

            // FoodTray가 Put 되었을 경우,
            // FoodTray에 FoodTray가 들어오는 경우의 수는 이미 막아놓았다. 
            if (iFood.GameObject.TryGetComponent<FoodTray>(out FoodTray foodTray2))
            {
                base.Put(foodTray2);

                uIComponent.AddRange(foodTray2.Ingredients);

                foodTray2.GetObject = combinedFood;
            } 
            else
            {
                if (HasObject() && getObject.TryGetComponent<FoodTray>(out FoodTray foodTray3))
                {
                    foodTray3.GetObject = combinedFood;
                } 
                else
                {
                    GetObject = combinedFood;
                }
                Destroy(iFood.GameObject);
                uIComponent.AddRange(iFood.Ingredients);
            }
        }
    }

    protected bool TryGetCombinedRecipe(IFood iFood, out Recipe recipe)
    {
        recipe = null;
        List<EObjectSerialCode> ingredients = new List<EObjectSerialCode>();
        ingredients.AddRange(iFood.Ingredients);
        if (TryGet<FoodTray>(out FoodTray foodTray))
        {
            ingredients.Add(foodTray.SerialCode);
        } else
        {
            ingredients.AddRange(Ingredients);
        }

        RecipeManager.Instance.TryGetRecipe(ECookingMethod.Combine, ingredients, out recipe);
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
