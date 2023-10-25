using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 모든 조리 도구의 상위 클래스 
public abstract class Cookware : FixedContainer, IStateUIAttachable
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

    protected Image stateImage;

    // Property
    private ECookingMethod CookingMethod
    {
        get
        {
            if(HasObject() && getObject.TryGetComponent<CookableTray>(out CookableTray cookableTray))
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

                    if (!Equals(StateUIAttachable))
                    {
                        StateUIAttachable.OnProgressBegin();
                    }

                    OnProgressBegin();
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
        StopSelectedCoroutine();
        base.Remove();
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
                    if(getIFood.CurrCookingRate == 0)
                    {
                        if (RecipeManager.Instance.TryGetRecipe(CookingMethod, Ingredients, out Recipe currRecipe))
                        {
                            if (selectedCoroutine != null)
                            {
                                StopCoroutine(selectedCoroutine);
                            }

                            Destroy(getIFood.GameObject);
                            GameObject cookedPrefab = SerialCodeDictionary.Instance.FindBySerialCode(currRecipe.CookedFood);
                            getIFood = Instantiate(cookedPrefab).GetComponent<IFood>();

                            TopContainer.GetObject = getIFood.GameObject.GetComponent<InteractableObject>();
                        }else return false;
                    }
                    selectedCoroutine = CookCoroutine(getIFood);
                    StartCoroutine(selectedCoroutine);
                    return true;
                } 
            }
        }
        return false;
    }

    protected IEnumerator CookCoroutine(IFood cookingFood)
    {
        cookwareState = ECookwareState.Cook;

        // UI
        IStateUIAttachable stateUIAttachable = StateUIAttachable;
        if (stateUIAttachable.StateUI == null)
        {
            Image showImage = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Img_Progress).GetComponent<Image>();
            stateUIAttachable.StateUI = showImage.InstantiateOnCanvas();
        }
        Image gauge = stateUIAttachable.StateUI.transform.GetChild(1).GetComponent<Image>();

        if(!Equals(stateUIAttachable))
        {
            stateUIAttachable.OnProgressBegin();
        }
        OnProgressBegin();
        cookingFood.OnCooking();

        while (cookingFood.CurrCookingRate <= 100)
        {
            cookingFood.CurrCookingRate += (int)((1 / totalCookDuration) * 10);
            gauge.fillAmount = (float)cookingFood.CurrCookingRate / 100;
            yield return workInterval;
        }
        cookingFood.CurrCookingRate = 0;

        //if (!Equals(stateUIAttachable))
        //{
        //    stateUIAttachable.OnProgressEnd();
        //}
        //OnProgressEnd();
        cookingFood.OnCooked();

        cookwareState = ECookwareState.Complete;

        //AttachFoodImage();

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
        if (!Equals(StateUIAttachable))
        {
            StateUIAttachable.OnProgressEnd();
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

        FoodUIComponent foodUIComponent = FoodUIAttachable.FoodUIComponent;
        foodUIComponent.Clear(true);

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
