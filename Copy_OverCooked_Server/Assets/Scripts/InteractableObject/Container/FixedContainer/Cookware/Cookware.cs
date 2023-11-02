using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 모든 조리 도구의 상위 클래스 
public abstract class Cookware : FixedContainer, IProgressUIAttachable
{
    [Header("Cookware")]
    [SerializeField]
    protected float totalCookDuration = 5f;
    [SerializeField]
    protected ECookingMethod cookingMethod;
    [SerializeField]
    protected ECookwareState cookwareState = ECookwareState.Idle;

    // 단위 시간 당 요리 시간 
    private WaitForSeconds workInterval = new WaitForSeconds(0.1f);

    // 현재 선택된 코루틴
    // 요리하는 코루틴과 StateUI 실행시키는 코루틴, 2가지가 있음 
    protected IEnumerator selectedCoroutine;

    protected Image progressImage;
    protected Image stateImage;

    protected float currTotalCookDuration;

    // Property
    protected virtual ECookingMethod CookingMethod
    {
        get => cookingMethod;
    }

    public ECookwareState CookwareState 
    { 
        get => cookwareState; 
        set 
        { 
            cookwareState = value; 
        } 
    }

    public Image ProgressImage
    {
        get
        {
            return progressImage;
        }
        set
        {
            progressImage = value;
            if(progressImage != null)
            {
                progressImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + UIOffset;
            }
        }
    }

    public Image StateImage
    {
        get
        {
            return stateImage;
        }
        set
        {
            stateImage = value;
            if (stateImage != null)
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
    
    public IProgressUIAttachable ProgressUIAttachable
    {
        get
        {
            if (getObject != null && getObject.TryGet<IProgressUIAttachable>(out IProgressUIAttachable component))
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
            if (Flammablity && TryGet<IFood>(out IFood getFood))
            {
                if(getFood.CookingMethod == CookingMethod)
                {
                    if (getFood.CurrOverTime > 0)
                    {
                        if (!Equals(ProgressUIAttachable))
                        {
                            ProgressUIAttachable.OnProgressBegin();
                        }

                        OnProgressBegin();

                        SelectCoroutine(AddUIStateEvent());
                    }
                }
            }
            return true;
        }
        return false;
    }

    public override void Remove(InteractableObject interactableObject)
    {
        //if(StateUIAttachable.ProgressImage != null && cookwareState == ECookwareState.Complete)
        //{
        //    Destroy(StateUIAttachable.ProgressImage.gameObject);
        //    StateUIAttachable.ProgressImage = null;
        //}
        StopSelectedCoroutine();
        base.Remove(interactableObject);

        if(progressImage != null)
        {
            Destroy(progressImage.gameObject);
            progressImage = null;
        }

        if(stateImage != null)
        {
            Destroy(stateImage.gameObject);
            stateImage = null;
        }

        cookwareState = ECookwareState.Idle;
    }

    protected bool TryCook()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            if (CanCook() && TryGet<IFood>(out IFood getIFood))
            {
                if(getIFood.CookingMethod != CookingMethod)
                {
                    if (RecipeManager.Instance.TryGetRecipe(CookingMethod, Ingredients, out Recipe currRecipe))
                    {
                        Destroy(getIFood.GameObject);
                        GameObject cookedPrefab = SerialCodeDictionary.Instance.FindBySerialCode(currRecipe.CookedFood);
                        getIFood = Instantiate(cookedPrefab).GetComponent<IFood>();

                        TopContainer.GetObject = getIFood.GameObject.GetComponent<InteractableObject>();
                    } else
                        return false;
                }
                else if (getIFood.CurrCookingRate >= 100)
                {
                    return false;
                }

                currTotalCookDuration = totalCookDuration;
                SelectCoroutine(CookCoroutine(getIFood));
                return true;
            }
        }
        return false;
    }

    protected IEnumerator CookCoroutine(IFood cookingFood)
    {
        cookwareState = ECookwareState.Cook;

        // UI
        IProgressUIAttachable progressUIAttachable = ProgressUIAttachable;
        if (progressUIAttachable.ProgressImage == null)
        {
            Image showImage = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Img_Progress).GetComponent<Image>();
            progressUIAttachable.ProgressImage = showImage.InstantiateOnCanvas();
        }
        Image gauge = progressUIAttachable.ProgressImage.transform.GetChild(1).GetComponent<Image>();

        if(!Equals(progressUIAttachable))
        {
            progressUIAttachable.OnProgressBegin();
        }
        OnProgressBegin();
        cookingFood.OnCooking();

        while (cookingFood.CurrCookingRate <= currTotalCookDuration)
        {
            cookingFood.CurrCookingRate += 0.1f;
            gauge.fillAmount = (float)cookingFood.CurrCookingRate / currTotalCookDuration;
            yield return workInterval;
        }

        cookingFood.OnCooked();

        cookwareState = ECookwareState.Complete;

        if (!FoodUIAttachable.FoodUIComponent.HasImage)
        {
            FoodUIAttachable.AddIngredientImages();
        }

        if(progressUIAttachable.ProgressImage != null)
        {
            Destroy(progressUIAttachable.ProgressImage.gameObject);
            progressUIAttachable.ProgressImage = null;
        }
        

        if (Flammablity)
        {
            SelectCoroutine(AddUIStateEvent());
        }
    }


    private IEnumerator AddUIStateEvent()
    {
        EventManager.Instance.AddEvent(new UIStateEvent(this));

        getObject.TryGet<Food>(out Food getFood);
        while (getFood != null && getFood.CurrOverTime <= 100 && (flammablity || getFood.CurrOverTime < 60))
        {
            getFood.CurrOverTime += 1;
            yield return workInterval;
        }
    }

    protected void SelectCoroutine(IEnumerator selectCoroutine)
    {
        if (selectedCoroutine != null)
        {
            StopCoroutine(selectedCoroutine);
        }
        selectedCoroutine = selectCoroutine;
        StartCoroutine(selectedCoroutine);
    }

    public void StopSelectedCoroutine()
    {
        if (!Equals(ProgressUIAttachable))
        {
            ProgressUIAttachable.OnProgressEnd();
        }
        OnProgressEnd();

        if (selectedCoroutine != null)
        {
            StopCoroutine(selectedCoroutine);
            selectedCoroutine = null;
        }
    }

    public void OnOverheat()
    {
        cookwareState = ECookwareState.Overheat;

        FoodUIAttachable.FoodUIComponent.Clear(true);

        if(TryGet<IFood>(out IFood food))
        {
            food.OnBurned();
        }

        if (Flammablity)
        {
            fireTriggerBox.Ignite();
        }
    } 

    protected abstract bool CanCook();

    public abstract void OnProgressBegin();

    public abstract void OnProgressEnd();
}
