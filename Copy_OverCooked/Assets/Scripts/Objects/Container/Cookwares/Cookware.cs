using System.Collections;
using UnityEngine;

public enum ECookwareState
{
    Idle,
    Cook,
    Complete,
    Overheat
}

public abstract class Cookware : Container
{
    [SerializeField]
    protected CookingMethod cookingMethod;
    [SerializeField]
    private bool IsImmediateCook = true;

    protected ECookwareState cookwareState;

    //protected float currProgressTime;

    private bool STOP = false;

    private void Awake()
    {
        cookwareState = ECookwareState.Idle;
    }

    protected virtual bool TryCook()
    {
        if (!getObject.TryGetComponent<Food>(out Food cookFood))
        {
            Debug.Log("Object don't have \"Food\"");
            return false;
        }
        float currProgressTime = cookFood.currentCookTime;
        Recipe recipe = RecipeManager.Instance.Search(cookingMethod, containObjects);
        if (recipe == null)
        {
            Debug.Log("Invalid Recipe");
            return false;
        }
    
        STOP = false;
        cookwareState = ECookwareState.Cook;
        float totalCookDuration = recipe.getTotalCookDuration();

        StartCoroutine(Cook(currProgressTime, totalCookDuration, recipe.getCookedFood()));
        return true;
    }

    private IEnumerator Cook(float currDuration, float totalDuration, Food cookedFood)
    {
        // 조리 도구마다 방식이 다름 
        // 조리가 진행중인 음식이 들어올 수도 있음 = Get() 함수 override 해야할 것 
        //float? test2 = null;
        //test2 ??= recipe?.getCookTime() ?? 0f;

        Food cookingFood = getObject.GetComponent<Food>();

        while (currDuration < totalDuration)
        {
            if (STOP)
            {
                cookingFood.currentCookTime = currDuration;
                yield break;
            }

            currDuration += 0.1f;
            int percentage = (int)((currDuration / totalDuration) * 100);
            if(percentage > 100) percentage = 100;
            Debug.Log($"<color=green> Cooking {cookingFood.name}... {percentage}% </color>");
            yield return new WaitForSeconds(0.1f);
        }
        cookingFood.currentCookTime = currDuration;

        LinkManager.Instance.GetLinkedPlayer(this).GetInteractor().RemoveObject(getObject);
        Destroy(getObject.gameObject);

        containObjects.Clear();
        getObject = Instantiate(cookedFood.gameObject, transform.position + offset, Quaternion.identity).GetComponent<InteractableObject>();
        getObject.IsInteractable = false;

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

    public override bool Put(InteractableObject interactableObject)
    {
        if (base.Put(interactableObject))
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
