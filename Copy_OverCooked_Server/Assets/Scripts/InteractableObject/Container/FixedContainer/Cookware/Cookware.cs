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
    private ECookingMethod CookingMethod
    {
        get
        {
            if(HasObject() && getObject.TryGetComponent<ICookableTray>(out ICookableTray cookableTray))
            {
                return cookableTray.CookingMethod;
            }
            return cookingMethod;
        }
    }
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

    private List<EObjectSerialCode> Ingredients
    {
        get
        {
            if(HasObject() && getObject.TryGet<Tray>(out Tray tray))
            {
                return tray.Ingredients;
            }
            IFood food = getObject as IFood;
            return food.Ingredients;
        }
    }

    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (Flammablity && getObject.TryGet<IFood>(out IFood getFood))
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
            if (CanCook() && getObject.TryGet<IFood>(out IFood getIFood))
            {
                if (getIFood.CurrCookingRate < 100)
                {
                    if (RecipeManager.Instance.TryGetRecipe(CookingMethod, Ingredients, out Recipe currRecipe))
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

        IFood cookedFood = SerialCodeDictionary.Instance.FindBySerialCode(recipe.CookedFood).GetComponent<IFood>();
        if (!getObject.TryGet<IFood>(out IFood currFood))
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
            //Debug.Log($"Cooking... <color=yellow>{currFood.GameObject.name}</color> => <color=orange>{cookedFood.GameObject.name}</color> <color=green>## {currFood.CurrCookingRate}%</color>");
            yield return workInterval;
        }

        cookwareState = ECookwareState.Complete;

        InteractableObject instantiateFood = Instantiate(cookedFood.GameObject, currFood.GameObject.transform.position, Quaternion.identity).GetComponent<InteractableObject>();

        Destroy(currFood.GameObject);

        TopContainer.GetObject = instantiateFood;

        if (!FoodUIAttachable.FoodUIComponent.HasImage)
        {
            FoodUIAttachable.AddIngredientImages();
        }

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
        foodUIComponent.Clear(true);

        if(TryGet<Food>(out Food food))
        {
            food.OnBurned();
        }

        if (Flammablity)
        {
            fireTriggerBox.Ignite();
        }
    }

    protected abstract bool CanCook();
}
