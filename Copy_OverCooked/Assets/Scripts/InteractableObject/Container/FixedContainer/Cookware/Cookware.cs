using System;
using System.Collections;
using System.Collections.Generic;
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

    protected override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
        if(getObject.TryGet<Food>(out Food getFood))
        {
            float currCookDegree = getFood.currCookDegree;
            if(currCookDegree >= 100)
            {
                if (currSelectedCoroutine != null)
                {
                    StopCoroutine(currSelectedCoroutine);
                }
                currSelectedCoroutine = AddUIStateEvent();
                StartCoroutine(currSelectedCoroutine);
            }
        }
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
            Debug.Log("111");
            if (CanCook() && getObject.TryGet<Food>(out Food getFood))
            {
                Debug.Log("222");
                if (getFood.currCookDegree < 100)
                {
                    if (getObject.TryGetComponent<Tray>(out Tray tray))
                    {
                        containObjects = tray.containObjects;
                    }
                    if (RecipeManager.Instance.TryGetRecipe(cookingMethod, containObjects, out Recipe currRecipe))
                    {
                        Debug.Log("444");
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
            Debug.Log($"Cooking... <color=yellow>{currFood.name}</color> => <color=red>{cookedFood.name}</color> <color=green>## {currFood.currCookDegree}%</color>");
            yield return waitForTick;
        }
        containObjects.Clear();
        cookedFood.currCookDegree = 100;
        Utill.Convert(ref getObject, cookedFood);
        
        cookwareState = ECookwareState.Complete;
        currSelectedCoroutine = AddUIStateEvent();
        StartCoroutine(currSelectedCoroutine);
    }

    private IEnumerator AddUIStateEvent()
    {
        EventManager.Instance.AddEvent(new UIStateEvent(this));
        Food getFood = getObject as Food;
        while(true)
        {
            getFood.currCookDegree += 1;
            yield return waitForTick;
        }
    }

    //private IEnumerator UIStateCoroutine()
    //{
    //    UIStateTimer uIStateTimer = new UIStateTimer(this);
    //    Food getFood = getObject as Food;
    //    IEnumerator currUICoroutine = null;
    //    while(getObject != null)
    //    {
    //        IEnumerator getCoroutine = uIStateTimer.GetCoroutine(getFood.currCookDegree);
    //        if (getCoroutine != null) 
    //        {
    //            if (currUICoroutine != null)
    //            {
    //                StopCoroutine(currUICoroutine);
    //            }
    //            if(uIImage != null)
    //            {
    //                Destroy(uIImage.gameObject);
    //            }
    //            currUICoroutine = getCoroutine;
    //            StartCoroutine(currUICoroutine);
    //        }
    //        getFood.currCookDegree += 1;
    //        yield return waitForTick;
    //    }
    //    currSelectedCoroutine = null;
    //    if(uIImage != null)
    //    {
    //        Destroy(uIImage.gameObject);
    //    }
    //}

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
