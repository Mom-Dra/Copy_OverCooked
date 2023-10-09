using System.Collections;
using System.Collections.Generic;
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

    protected float currProgressTime;

    private bool STOP = false;

    private void Awake()
    {
        cookwareState = ECookwareState.Idle;
    }

    private IEnumerator Cook()
    {
        // 조리 도구마다 방식이 다름 
        // 조리가 진행중인 음식이 들어올 수도 있음 = Get() 함수 override 해야할 것 
        Recipe recipe = RecipeManager.Instance.Search(cookingMethod, containObjects);
        Debug.Log(recipe);
        float cookDuration = recipe.getCookTime();
        currProgressTime = 0;

        while (currProgressTime < cookDuration)
        {
            if (STOP)
                yield break;

            currProgressTime += 0.1f;
            Debug.Log($"Progress: {currProgressTime} / {cookDuration}%");
            yield return new WaitForSeconds(0.1f);
        }

        containObjects.Clear();
        getObject.RemoveFromInteractor();
        getObject = Instantiate(recipe.getCookedFood().gameObject, transform.position + offset, Quaternion.identity).GetComponent<InteractableObject>();
        if (getObject == null)
        {
            Debug.Log("Invalid Component : 'Food'");
        }
        else
            cookwareState = ECookwareState.Complete;
    }

    protected void StopCook()
    {
        STOP = true;
    }

    protected void StartCook()
    {
        STOP = false;
        cookwareState = ECookwareState.Cook;
        StartCoroutine(Cook());
    }

    public void Interact()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            StartCook();
        }
    }


    //public InteractableObject getCookedObject()
    //{
    //    cookedFood.GetComponent<Food>().IsInteractable = true;
    //    InteractableObject go = cookedFood;
    //    cookedFood = null;
    //    return go;
    //}

    public override bool Put(InteractableObject gameObject)
    {
        if (base.Put(gameObject))
        {
            if (IsImmediateCook && IsFull())
            {
                StartCook();
            }
            return true;
        }
        return false;
    }
}
