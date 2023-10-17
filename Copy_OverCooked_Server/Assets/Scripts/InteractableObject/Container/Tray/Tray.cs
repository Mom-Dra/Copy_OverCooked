using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tray : Container
{
    [Header("Tray")]
    [SerializeField]
    private Vector3 stateUIOffset = Vector3.down;
    [SerializeField]
    private bool PlusBaseUI = true;

    [SerializeField]
    private List<Food> ingredients = new List<Food>();

    protected UIComponent uIComponent;
    protected Image stateImage;

    public List<Food> Ingredients
    {
        get => ingredients;
    }

    public UIComponent UIComponent
    {
        get => uIComponent;
    }

    public Image StateImage
    {
        get => stateImage;
        set
        {
            stateImage = value;
            stateImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + UIOffset;
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
            uIComponent = new UIComponent(transform, uIOffset);
        }
        base.Awake();
    }

    private void FixedUpdate()
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

    public override void Put(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<Food>(out Food food))
        {
            if (food.UIComponent.HasImage)
            {
                food.UIComponent.Clear();
            }
            if (ingredients.Count == 0)
            {
                base.Put(food);
                ingredients.Add(food);
            } 
            else
            {
                List<Food> totalFoods = new List<Food> { food };
                totalFoods.AddRange(ingredients);
                if (TryGetCombinedRecipe(totalFoods, out Recipe recipe))
                {
                    foreach(Food destroyFood in totalFoods)
                    {
                        Destroy(destroyFood.gameObject);
                    }
                    Food combinedFood = Instantiate(SerialCodeDictionary.Instance.FindBySerialCode(recipe.CookedFood).GetComponent<Food>());
                    base.Put(combinedFood);
                    ingredients.Add(combinedFood);
                }
            }
            foreach(EObjectSerialCode serialCode in food.Ingredients)
            {
                uIComponent.Add(serialCode);
            }
        }
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent<Food>(out Food food))
        {
            if (ingredients.Count == 0)
                return true;

            List<Food> totalFoods = new List<Food> { food };
            totalFoods.AddRange(ingredients);
            return TryGetCombinedRecipe(totalFoods, out Recipe recipe);
        }
        return false;
    }

    protected bool TryGetCombinedRecipe(List<Food> ingredients, out Recipe recipe)
    {
        recipe = null;
        RecipeManager.Instance.TryGetRecipe(ECookingMethod.Combine, ingredients, out recipe);
        return recipe != null;
    }

    public override void Remove()
    {
        base.Remove();
        ingredients.Clear();
        uIComponent.Clear();
    }
}
