using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ECookwareState
{
    Idle,
    Cook,
    Complete,
    Overheat
}

public abstract class Cookware : Container
{
    [Header("Cookware")]
    [SerializeField]
    protected ECookingMethod cookingMethod;
    [SerializeField]
    private bool IsImmediateCook = true;

    protected ECookwareState cookwareState = ECookwareState.Idle;

    private bool STOP = false;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(IsGrabbable && getObject != null)
        {
            getObject.transform.position = transform.position + containOffset;
        }   
    }

    protected virtual bool TryCook()
    {
        if (!getObject.TryGetComponent<Food>(out Food cookFood))
        {
            Debug.Log("Object don't have \"<Food>\"");
            return false;
        }
        float currProgressTime = cookFood.currentCookTime;
        if(containObjects.Count == 0 && getObject != null)
        {
            containObjects.Add(getObject);
        }
        
        Recipe recipe = RecipeManager.Instance.Search(cookingMethod, containObjects);
        if (recipe == null)
        {
            Debug.Log("Invalid Recipe");
            return false;
        }
    
        STOP = false;
        cookwareState = ECookwareState.Cook;

        StartCoroutine(Cook(currProgressTime, recipe));
        return true;
    }

    private IEnumerator Cook(float currDuration, Recipe recipe)
    {
        
        float totalDuration = recipe.getTotalCookDuration();
        Food getFood = getObject.GetComponent<Food>();

        containObjects.Clear();

        // UI Setting
        if (getFood.uIImage != null)
        {
            uIImage = getFood.uIImage;
            getFood.uIImage = null;
        }
        else
        {
            uIImage = UIManager.Instance.InstantiateUI(EInGameUIType.Progress);
        }
        Image progressGaugeImage = uIImage.transform.GetChild(1).GetComponent<Image>();

        // Extra Food(Prefab) Setting
        List<Food> extraFoods = recipe.getExtraFoods();

        int totalCount = 0; // 음식이 변하는 횟수 (Total)
        int changeUnit = 0; // 음식이 변하는 퍼센트 단위(간격)
        if(extraFoods != null &&  extraFoods.Count > 0)
        {
            totalCount = extraFoods.Count + 1;
            changeUnit = 100 / totalCount;
        }
        int currCount = (int)((currDuration / totalDuration) * 100) / changeUnit; // 음식이 변하는 횟수 (Current)


        // Cooking
        while (currDuration < totalDuration)
        {
            if (STOP)
            {
                yield break;
            }

            getFood.currentCookTime = currDuration += 0.1f;
            progressGaugeImage.fillAmount = currDuration / totalDuration;

            int percentage = (int)((currDuration / totalDuration) * 100);
            if (percentage >= 100)
            {
                break;
            }
            Debug.Log($"<color=green> Cooking { getFood.name}... {percentage}% </color>");

            // 조리하는 음식의 상태 변화를 새로운 Prefab으로 교체함으로 표현
            if(currCount < extraFoods.Count && percentage >= (currCount + 1) * changeUnit)
            {
                Vector3 instantiatePos = getObject.transform.position;
                Destroy(getObject.gameObject);
                getObject = Instantiate(extraFoods[currCount].gameObject, instantiatePos, Quaternion.identity).GetComponent<InteractableObject>();
                getObject.IsInteractable = false;
                getObject.Fix();
                Debug.Log($"<color=yellow> Change [{currCount}] : {getObject} </color>");
                getFood = getObject.GetComponent<Food>();
                getFood.currentCookTime = currDuration;
                ++currCount;
            }

            yield return new WaitForSeconds(0.1f);
        }

        //LinkManager.Instance.GetLinkedPlayer(this).GetInteractor().RemoveObject(getObject);
        Destroy(getObject.gameObject);
        Destroy(uIImage.gameObject);

        
        getObject = Instantiate(recipe.getCookedFood(), transform.position + containOffset, Quaternion.identity).GetComponent<InteractableObject>();
        getObject.IsInteractable = false;
        getObject.Fix();

        if (getObject == null)
        {
            Debug.Log("Invalid Component : 'Food'");
        }
        else
        {
            cookwareState = ECookwareState.Complete;
        }
        StopCook();
    }

    protected virtual void StopCook()
    {
        getObject.GetComponent<Food>().uIImage = uIImage;
        uIImage = null;
        STOP = true;
        cookwareState = ECookwareState.Idle;
        LinkManager.Instance.Disconnect(this);
    }

    public void Interact()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            if(!TryCook()) 
                LinkManager.Instance.Disconnect(this);
        }
    }

    public override InteractableObject Get()
    {
        if(cookwareState == ECookwareState.Cook)
        {
            StopCook();
        }
        return base.Get();
    }

    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (IsImmediateCook && IsFull())
            {
                if (!TryCook())
                    LinkManager.Instance.Disconnect(this);
            }
            return true;
        }
        return false;
    }


}
