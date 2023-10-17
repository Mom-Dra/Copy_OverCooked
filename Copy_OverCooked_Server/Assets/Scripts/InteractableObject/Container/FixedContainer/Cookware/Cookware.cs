using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Cookware : FixedContainer, IStateUIAttachable
{
    [Header("Cookware")]
    [SerializeField]
    protected ECookingMethod cookingMethod;
    [SerializeField]
    protected ECookwareState cookwareState = ECookwareState.Idle;

    private WaitForSeconds workInterval = new WaitForSeconds(0.1f);
    private IEnumerator selectedCoroutine;

    [SerializeField]
    protected Image stateImage;

    // Property
    public ECookwareState CookwareState 
    { 
        get => cookwareState; 
        set 
        { 
            cookwareState = value; 
        } 
    }

    public Image StateUI
    {
        get
        {
            return stateImage;
        }
        set
        {
            stateImage = value;
            if(stateImage != null)
            {
                stateImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + UIOffset;
            }
        }
    }

    public IFoodUIAttachable FoodUIAttachable
    {
        get
        {
            if(getObject != null && getObject.TryGet<IFoodUIAttachable>(out IFoodUIAttachable component))
            {
                return component;
            }
            return null;
        }
    }
    
    public IStateUIAttachable StateUIAttachable
    {
        get
        {
            if (getObject != null && getObject.TryGet<IStateUIAttachable>(out IStateUIAttachable component))
            {
                return component;
            }
            return this;
        }
    }

    private List<Food> Ingredients
    {
        get
        {
            if(HasObject() && getObject.TryGet<Tray>(out Tray tray))
            {
                return tray.Ingredients;
            }
            Food food = getObject as Food;
            return new List<Food> { food };
        }
    }

    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (Flammablity && getObject.TryGet<Food>(out Food getFood))
            {
                if (getFood.CurrOverTime > 0)
                {
                    if (selectedCoroutine != null)
                    {
                        StopCoroutine(selectedCoroutine);
                    }
                    selectedCoroutine = AddUIStateEvent();
                    StartCoroutine(selectedCoroutine);
                }
            }
            return true;
        }
        return false;
    }

    public override void Remove()
    {
        if(StateUIAttachable.StateUI != null && cookwareState == ECookwareState.Complete)
        {
            Destroy(StateUIAttachable.StateUI.gameObject);
            StateUIAttachable.StateUI = null;
        }
        base.Remove();
        StopSelectedCoroutine();
        cookwareState = ECookwareState.Idle;
    }

    protected bool TryCook()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            if (CanCook() && getObject.TryGet<Food>(out Food getFood))
            {
                if (getFood.CurrCookingRate < 100)
                {
                    if (RecipeManager.Instance.TryGetRecipe(cookingMethod, Ingredients, out Recipe currRecipe))
                    {
                        if (selectedCoroutine != null)
                        {
                            StopCoroutine(selectedCoroutine);
                        }
                        selectedCoroutine = CookCoroutine(currRecipe);
                        StartCoroutine(selectedCoroutine);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    protected IEnumerator CookCoroutine(Recipe recipe)
    {
        cookwareState = ECookwareState.Cook;

        Food cookedFood = SerialCodeDictionary.Instance.FindBySerialCode(recipe.CookedFood).GetComponent<Food>();
        if (!getObject.TryGet<Food>(out Food currFood))
        {
            throw new System.Exception("Cookware warning : GetObject is not <Food>");
        }

        float totalCookDuration = recipe.TotalCookDuration;

        // UI
        IStateUIAttachable stateUIAttachable = StateUIAttachable;
        if (stateUIAttachable.StateUI == null)
        {
            Image showImage = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Img_Progress).GetComponent<Image>();
            stateUIAttachable.StateUI = showImage.InstantiateOnCanvas();
        }
        Image gauge = stateUIAttachable.StateUI.transform.GetChild(1).GetComponent<Image>();

        while (currFood.CurrCookingRate <= 100)
        {
            currFood.CurrCookingRate += (int)((1 / totalCookDuration) * 10);
            gauge.fillAmount = (float)currFood.CurrCookingRate / 100;
            Debug.Log($"Cooking... <color=yellow>{currFood.name}</color> => <color=orange>{cookedFood.name}</color> <color=green>## {currFood.CurrCookingRate}%</color>");
            yield return workInterval;
        }

        cookwareState = ECookwareState.Complete;

        Food instantiateFood = Instantiate(cookedFood, currFood.transform.position, Quaternion.identity);
        instantiateFood.CurrCookingRate = 0;

        Destroy(currFood.gameObject);

        TopContainer.GetObject = instantiateFood;

        FoodUIAttachable.AddIngredientImages();

        if(stateUIAttachable.StateUI != null)
        {
            Destroy(stateUIAttachable.StateUI.gameObject);
            stateUIAttachable.StateUI = null;
        }
        

        if (Flammablity)
        {
            if (selectedCoroutine != null)
            {
                StopCoroutine(selectedCoroutine);
            }
            selectedCoroutine = AddUIStateEvent();
            StartCoroutine(selectedCoroutine);
        }
    }


    private IEnumerator AddUIStateEvent()
    {
        cookwareState = ECookwareState.Complete;
        EventManager.Instance.AddEvent(new UIStateEvent(this));
        getObject.TryGet<Food>(out Food getFood);
        while (getFood != null && getFood.CurrOverTime <= 100 && (flammablity || getFood.CurrOverTime < 60))
        {
            getFood.CurrOverTime += 1;
            yield return workInterval;
        }
    }

    public void StopSelectedCoroutine()
    {
        if (selectedCoroutine != null)
        {
            StopCoroutine(selectedCoroutine);
            selectedCoroutine = null;
        }
    }

    public virtual void OnOverheat()
    {
        cookwareState = ECookwareState.Overheat;

        FoodUIComponent foodUIComponent = FoodUIAttachable.FoodUIComponent;
        foodUIComponent.Clear();
        foodUIComponent.Add(EObjectSerialCode.Img_Overheat);

        if(TryGet<Food>(out Food food))
        {
            food.OnBurned();
        }
    }

    protected abstract bool CanCook();
}
