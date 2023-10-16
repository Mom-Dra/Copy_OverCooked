using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Cookware : FixedContainer
{
    [Header("Cookware")]
    [SerializeField]
    protected ECookingMethod cookingMethod;
    [SerializeField]
    protected ECookwareState cookwareState = ECookwareState.Idle;

    private WaitForSeconds workInterval = new WaitForSeconds(0.1f);
    private IEnumerator selectedCoroutine;

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

    public Image StateImage
    {
        get
        {
            if (TryFind<Tray>(out Tray tray))
            {
                return tray.StateImage;
            } else
            {
                return stateImage;
            }
        }
        set
        {
            if(TryFind<Tray>(out Tray tray))
            {
                tray.StateImage = value;
            } 
            else
            {
                stateImage = value;
                stateImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + UIOffset;
            }
        }
    }

    private List<Food> Ingredients
    {
        get
        {
            if(HasObject() && getObject.TryFind<Tray>(out Tray tray))
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
            if (Flammablity && getObject.TryFind<Food>(out Food getFood))
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
        if(StateImage != null)
        {
            Destroy(StateImage.gameObject);
        }
        base.Remove();
        StopSelectedCoroutine();
        cookwareState = ECookwareState.Idle;
    }

    protected bool TryCook()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            if (CanCook() && getObject.TryFind<Food>(out Food getFood))
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
        getObject.TryFind<Food>(out Food currFood);
        if (currFood == null)
        {
            throw new System.Exception("Cookware warning : GetObject is not <Food>");
        }

        float totalCookDuration = recipe.TotalCookDuration;

        // UI
        //Image progressBar = InstantiateManager.Instance.InstantiateByUIType(this, EInGameUIType.Progress);
        if (StateImage == null)
        {
            Image showImage = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Img_Progress).GetComponent<Image>();
            StateImage = showImage.InstantiateOnCanvas();
        }
        Image gauge = StateImage.transform.GetChild(1).GetComponent<Image>();

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

        if(TryFind<Tray>(out Tray tray))
        {
            if(!tray.UIComponent.HasImage)
            {
                foreach (EObjectSerialCode serialCode in instantiateFood.Ingredients)
                {
                    tray.UIComponent.Add(serialCode);
                }
            }
        } 
        else
        {
            instantiateFood.AddUISelf();
        }

        GameObject.Destroy(StateImage.gameObject);

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
        EventManager.Instance.AddEvent(new UIStateEvent(this));
        getObject.TryFind<Food>(out Food getFood);
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

    protected abstract bool CanCook();
}
