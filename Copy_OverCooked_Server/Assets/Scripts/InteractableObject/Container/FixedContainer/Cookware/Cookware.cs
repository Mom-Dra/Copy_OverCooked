using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��� ���� ������ ���� Ŭ���� 
public abstract class Cookware : FixedContainer, IStateUIAttachable
{
    [Header("Cookware")]
    [SerializeField]
    protected float totalCookDuration = 5f;
    [SerializeField]
    protected ECookingMethod cookingMethod;
    [SerializeField]
    protected ECookwareState cookwareState = ECookwareState.Idle;

    // ���� �ð� �� �丮 �ð� 
    private WaitForSeconds workInterval = new WaitForSeconds(0.1f);

    // ���� ���õ� �ڷ�ƾ
    // �丮�ϴ� �ڷ�ƾ�� StateUI �����Ű�� �ڷ�ƾ, 2������ ���� 
    protected IEnumerator selectedCoroutine;

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
            if (Flammablity && TryGet<IFood>(out IFood getFood))
            {
                if(getFood.CookingMethod == CookingMethod)
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
            }
            return true;
        }
        return false;
    }

    public override void Remove(InteractableObject interactableObject)
    {
        if(StateUIAttachable.StateUI != null && cookwareState == ECookwareState.Complete)
        {
            Destroy(StateUIAttachable.StateUI.gameObject);
            StateUIAttachable.StateUI = null;
        }
        StopSelectedCoroutine();
        base.Remove(interactableObject);
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

                if (selectedCoroutine != null)
                {
                    StopCoroutine(selectedCoroutine);
                }

                currTotalCookDuration = totalCookDuration;
                selectedCoroutine = CookCoroutine(getIFood);
                StartCoroutine(selectedCoroutine);
                return true;
            }
        }
        return false;
    }

    protected IEnumerator CookCoroutine(IFood cookingFood, bool resetCurrCookTime = true)
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

        while (cookingFood.CurrCookingRate <= currTotalCookDuration)
        {
            cookingFood.CurrCookingRate += 0.1f;
            gauge.fillAmount = (float)cookingFood.CurrCookingRate / currTotalCookDuration;
            yield return workInterval;
        }

        //if(resetCurrCookTime)
        //    cookingFood.CurrCookingRate = 0;

        cookingFood.OnCooked();

        cookwareState = ECookwareState.Complete;

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
