using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
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

    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (getObject.TryGet<Food>(out Food getFood))
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

    public override void Remove(InteractableObject interactableObject)
    {
        if (UIImage != null)
        {
            if (cookwareState == ECookwareState.Overheat && getObject.TryGet<Food>(out Food getFood))
            {
                getFood.UIImage = UIImage;
            } 
            else
            {
                Destroy(UIImage.gameObject);
            }
            UIImage = null;
        }

        base.Remove(interactableObject);
        StopSelectedCoroutine();
        cookwareState = ECookwareState.Idle;
    }

    protected bool TryCook()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            if (CanCook() && getObject.TryGet<Food>(out Food getFood))
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
        getObject.TryGet<Food>(out Food currFood);
        if (currFood == null)
        {
            throw new System.Exception("Cookware warning : GetObject is not <Food>");
        }

        float totalCookDuration = recipe.TotalCookDuration;

        // UI
        if (UIImage == null)
        {
            UIImage = InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.Progress);
        }
        Image gauge = UIImage.transform.GetChild(1).GetComponent<Image>();

        while (currFood.CurrCookDegree <= 100)
        {
            currFood.CurrCookDegree += (int)((1 / totalCookDuration) * 10);
            gauge.fillAmount = (float)currFood.CurrCookDegree / 100;
            Debug.Log($"Cooking... <color=yellow>{currFood.name}</color> => <color=orange>{cookedFood.name}</color> <color=green>## {currFood.CurrCookDegree}%</color>");
            yield return workInterval;
        }
        containObjects.Clear();
        cookwareState = ECookwareState.Complete;

        Food instantiateFood = Instantiate(cookedFood, currFood.transform.position, Quaternion.identity);
        instantiateFood.CurrCookDegree = 100;
        instantiateFood.Selectable = false;
        base.Remove(currFood);
        Destroy(currFood.gameObject);
        TryPut(instantiateFood);
    }


    private IEnumerator AddUIStateEvent()
    {
        EventManager.Instance.AddEvent(new UIStateEvent(this));
        getObject.TryGet<Food>(out Food getFood);
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
