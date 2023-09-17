using System.Collections;
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
    protected CookingMethod cookingMethod;
    [SerializeField]
    private bool IsImmediateCook = true;

    protected ECookwareState cookwareState = ECookwareState.Idle;

    private bool STOP = false;

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

        StartCoroutine(Cook(currProgressTime, recipe));
        return true;
    }

    private IEnumerator Cook(float currDuration, Recipe recipe)
    {
        float totalDuration = recipe.getTotalCookDuration();

        Food getFood = getObject.GetComponent<Food>();

        if (getFood.uIImage)
        {
            uIImage = getFood.uIImage;
            getFood.uIImage = null;
        }
        else
        {
            uIImage = UIManager.Instance.InstantiateUI(EInGameUIType.Progress);
        }
        Image progressGaugeImage = uIImage.transform.GetChild(1).GetComponent<Image>();

        while (currDuration < totalDuration)
        {
            if (STOP)
            {
                yield break;
            }

            getFood.currentCookTime = currDuration += 0.1f;
            progressGaugeImage.fillAmount = currDuration / totalDuration;
            int percentage = (int)((currDuration / totalDuration) * 100);
            if(percentage > 100) percentage = 100;
            Debug.Log($"<color=green> Cooking { getFood.name}... {percentage}% </color>");
            yield return new WaitForSeconds(0.1f);
        }

        LinkManager.Instance.GetLinkedPlayer(this).GetInteractor().RemoveObject(getObject);
        Destroy(getObject.gameObject);
        Destroy(uIImage.gameObject);

        containObjects.Clear();
        getObject = Instantiate(recipe.getCookedFood(), transform.position + containOffset, Quaternion.identity).GetComponent<InteractableObject>();
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
        Debug.Log("Stop : " + getObject);
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
