using System.Collections;
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

    // Property
    public ECookwareState CookwareState 
    { 
        get => cookwareState; 
        set 
        { 
            cookwareState = value; 
        } 
    }

    public override bool TryPut(SendObjectArgs sendContainerArgs)
    {
        if (base.TryPut(sendContainerArgs))
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
                    if (RecipeManager.Instance.TryGetRecipe(cookingMethod, ContainObjects, out Recipe currRecipe))
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
        Food cookedFood = recipe.CookedFood;
        getObject.TryFind<Food>(out Food currFood);
        if (currFood == null)
        {
            throw new System.Exception("Cookware warning : GetObject is not <Food>");
        }

        float totalCookDuration = recipe.TotalCookDuration;

        // UI
        //Image progressBar = InstantiateManager.Instance.InstantiateByUIType(this, EInGameUIType.Progress);
        if (!uIComponent.HasImage)
        {
            uIComponent.Add(EObjectSerialCode.Img_Progress);
        }
        Image gauge = uIComponent.FirstImage.transform.GetChild(1).GetComponent<Image>();

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

        if(TryFind<Tray>(out Tray tray))
        {
            tray.GetObject = instantiateFood;
        } 
        else
        {
            GetObject = instantiateFood;
            //instantiateFood.UIComponent.Add(inst);
        }

        uIComponent.Clear();

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
