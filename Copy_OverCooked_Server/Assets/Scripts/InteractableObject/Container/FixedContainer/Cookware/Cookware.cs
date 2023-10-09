using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class Cookware : FixedContainer
{
    public ECookwareState cookwareState = ECookwareState.Idle;
    [SerializeField]
    protected ECookingMethod cookingMethod;


    private WaitForSeconds waitForTick = new WaitForSeconds(0.1f);
    private IEnumerator currSelectedCoroutine;

    protected override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
        if(getObject.TryGet<Food>(out Food getFood))
        {
            int currCookDegree = getFood.currCookDegree;
            if(currCookDegree < 100)
            {
                
            }
        }
    }

    protected bool TryCook()
    {
        if(cookwareState == ECookwareState.Idle)
        {
            if (CanCook() && RecipeManager.Instance.TryGetRecipe(cookingMethod, containObjects, out Recipe currRecipe))
            {
                currSelectedCoroutine = Cook(currRecipe);
                StartCoroutine(currSelectedCoroutine);
                return true;
            }
        }
        return false;
    }

    protected IEnumerator Cook(Recipe recipe)
    {
        // 실질적으로 조리를 시작하는 코드
        Food currFood = getObject as Food;
        Food cookedFood = recipe.getCookedFood();
        if(currFood == null)
        {
            throw new System.Exception("Cookware warning : GetObject is not <Food>");
        }

        int currCookDegree = currFood.currCookDegree;
        float totalCookDuration = recipe.getTotalCookDuration();

        // UI
        uIImage = InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.Progress);
        Image gauge = uIImage.transform.GetChild(1).GetComponent<Image>();

        while (currCookDegree < totalCookDuration)
        {
            int percentage = (int)((currCookDegree / totalCookDuration) * 100);
            currFood.currCookDegree = percentage;
            gauge.fillAmount = currCookDegree / totalCookDuration;
            Debug.Log($"Cooking... <color=yellow>{currFood.name}</color> => <color=red>{cookedFood.name}</color> <color=green>## {percentage}%</color>");
            yield return waitForTick;
        }
        containObjects.Clear();
        Utill.Convert(ref getObject, cookedFood);
        
        cookwareState = ECookwareState.Complete;
        StartCoroutine(CompleteCoroutine());
    }

    private IEnumerator CompleteCoroutine()
    {
        Destroy(uIImage.gameObject);
        uIImage = InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.Complete);
        yield return new WaitForSeconds(4f);
        StartCoroutine(WarningCoroutine());
    }

    private IEnumerator WarningCoroutine()
    {
        Destroy(uIImage.gameObject);
        uIImage = InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.Warning);
        yield return new WaitForSeconds(3f);
        Overheat();
    }

    private void Overheat()
    {
        Destroy(uIImage.gameObject);
        uIImage = InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.Overheat);
    }

    public void StopCook()
    {
        StopCoroutine(currSelectedCoroutine);
        currSelectedCoroutine = null;
        cookwareState = ECookwareState.Idle;
    }

    protected abstract bool CanCook();
}
