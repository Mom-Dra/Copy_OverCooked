using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class Cookware : FixedContainer
{
    public ECookwareState cookwareState = ECookwareState.Idle;
    [Header("Cookware")]
    [SerializeField]
    protected ECookingMethod cookingMethod;
    

    private WaitForSeconds waitForTick = new WaitForSeconds(0.1f);
    private IEnumerator currSelectedCoroutine;

    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            Debug.Log("111");
            if (getObject.TryGet<Food>(out Food getFood))
            {
                Debug.Log("222");
                float currCookDegree = getFood.currCookDegree;
                if (currCookDegree >= 100)
                {
                    if (currSelectedCoroutine != null)
                    {
                        StopCoroutine(currSelectedCoroutine);
                    }
                    currSelectedCoroutine = AddUIStateEvent();
                    StartCoroutine(currSelectedCoroutine);
                }
            }
            return true;
        }
        return false;
    }

    public override void Remove(InteractableObject interactableObject)
    {
        base.Remove(interactableObject);
        if(getObject == null)
        {
            StopCook();
        }
    }

    protected bool TryCook()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            if (CanCook() && getObject.TryGet<Food>(out Food getFood))
            {
                if (getFood.currCookDegree < 100)
                {
                    if (getObject.TryGetComponent<Tray>(out Tray tray))
                    {
                        containObjects = tray.containObjects;
                    }
                    if (RecipeManager.Instance.TryGetRecipe(cookingMethod, containObjects, out Recipe currRecipe))
                    {
                        if (currSelectedCoroutine != null)
                        {
                            StopCoroutine(currSelectedCoroutine);
                        }
                        currSelectedCoroutine = CookCoroutine(currRecipe);
                        StartCoroutine(currSelectedCoroutine);
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
        Food cookedFood = recipe.getCookedFood();
        getObject.TryGet<Food>(out Food currFood);
        if(currFood == null)
        {
            throw new System.Exception("Cookware warning : GetObject is not <Food>");
        }
        
        float totalCookDuration = recipe.getTotalCookDuration();

        // UI
        if(uIImage == null)
        {
            uIImage = InstantiateManager.Instance.InstantiateUI(this, EInGameUIType.Progress);
        }
        Image gauge = uIImage.transform.GetChild(1).GetComponent<Image>();

        while (currFood.currCookDegree <= 100)
        {
            currFood.currCookDegree += (int)((1 / totalCookDuration) * 10);
            gauge.fillAmount = (float)currFood.currCookDegree / 100;
            Debug.Log($"Cooking... <color=yellow>{currFood.name}</color> => <color=orange>{cookedFood.name}</color> <color=green>## {currFood.currCookDegree}%</color>");
            yield return waitForTick;
        }
        containObjects.Clear();
        cookwareState = ECookwareState.Complete;

        Food instantiateFood = Instantiate(cookedFood, currFood.transform.position, Quaternion.identity);
        instantiateFood.currCookDegree = 100;
        instantiateFood.IsInteractable = false;
        base.Remove(currFood);
        Destroy(currFood.gameObject);
        TryPut(instantiateFood);
    }


    private IEnumerator AddUIStateEvent()
    {
        EventManager.Instance.AddEvent(new UIStateEvent(this));
        getObject.TryGet<Food>(out Food getFood);
        while(getFood != null && (IsFirable || getFood.currCookDegree < 160))
        {
            getFood.currCookDegree += 1;
            yield return waitForTick;
        }
    }

    public void StopCook()
    {
        if(currSelectedCoroutine != null)
        {
            StopCoroutine(currSelectedCoroutine);
            currSelectedCoroutine = null;
        }
        if(cookwareState != ECookwareState.Cook &&  uIImage != null)
        {
            Destroy(uIImage.gameObject);
            uIImage = null;
        }
        cookwareState = ECookwareState.Idle;
    }

    protected abstract bool CanCook();
}
