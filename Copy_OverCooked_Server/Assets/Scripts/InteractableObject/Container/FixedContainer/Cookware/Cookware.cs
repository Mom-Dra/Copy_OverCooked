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

    public override bool TryPut(SendContainerArgs sendContainerArgs)
    {
        if (base.TryPut(sendContainerArgs))
        {
            if (getObject.TryFind<Food>(out Food getFood))
            {
                if (getFood.CurrCookDegree >= 100)
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
                if (getFood.CurrCookDegree < 100)
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
        // 실질적으로 조리를 시작하는 코드
        Food cookedFood = recipe.CookedFood;
        getObject.TryFind<Food>(out Food currFood);
        if (currFood == null)
        {
            throw new System.Exception("Cookware warning : GetObject is not <Food>");
        }

        float totalCookDuration = recipe.TotalCookDuration;

        // UI
        Image progressBar = InstantiateManager.Instance.InstantiateByUIType(this, EInGameUIType.Progress);
        if (!uIComponent.HasImage)
        {
            uIComponent.Add(progressBar);
        }
        Image gauge = uIComponent.FirstImage.transform.GetChild(1).GetComponent<Image>();

        while (currFood.CurrCookDegree <= 100)
        {
            currFood.CurrCookDegree += (int)((1 / totalCookDuration) * 10);
            gauge.fillAmount = (float)currFood.CurrCookDegree / 100;
            Debug.Log($"Cooking... <color=yellow>{currFood.name}</color> => <color=orange>{cookedFood.name}</color> <color=green>## {currFood.CurrCookDegree}%</color>");
            yield return workInterval;
        }
        ContainObjects.Clear();
        cookwareState = ECookwareState.Complete;

        Food instantiateFood = Instantiate(cookedFood, currFood.transform.position, Quaternion.identity);
        instantiateFood.CurrCookDegree = 100;
        instantiateFood.Selectable = false;
        Destroy(currFood.gameObject);
        if(TryFind<Tray>(out Tray tray))
        {
            tray.Remove();
            tray.Put(instantiateFood);
        } 
        else
        {
            Put(instantiateFood);
            instantiateFood.UIComponent.Add(InstantiateManager.Instance.InstantiateOnCanvas(instantiateFood.GetFoodImage()));
        }
        //base.Remove();
        //TryPut(instantiateFood);
        if (selectedCoroutine != null)
        {
            StopCoroutine(selectedCoroutine);
        }
        selectedCoroutine = AddUIStateEvent();
        StartCoroutine(selectedCoroutine);
    }


    private IEnumerator AddUIStateEvent()
    {
        EventManager.Instance.AddEvent(new UIStateEvent(this));
        getObject.TryFind<Food>(out Food getFood);
        while (getFood != null && getFood.CurrCookDegree <= 200 && (flammablity || getFood.CurrCookDegree < 160))
        {
            getFood.CurrCookDegree += 1;
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
