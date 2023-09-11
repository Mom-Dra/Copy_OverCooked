using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum ECookwareState
{
    Idle,
    Cook,
    Complete,
    Overheat
}

public abstract class Cookware : InteractableObject, Containable
{
    [SerializeField]
    protected int requiredFoodCount;
    [SerializeField]
    protected Vector3 cookedOffset;
    [SerializeField]
    protected CookingMethod cookingMethod;
    [SerializeField]
    private bool isGrabbable;
    [SerializeField]
    private bool IsImmediateCook = true;

    protected ECookwareState cookwareState;
    protected List<Food> containFoods;

    protected GameObject cookedFood;
    protected float currProgressTime;

    private bool STOP = false;

    private void Awake()
    {
        cookwareState = ECookwareState.Idle;
        containFoods = new List<Food>(requiredFoodCount);
    }

    private bool Empty()
    {
        return containFoods.Count == 0;
    }

    private bool Full()
    {
        return containFoods.Count == requiredFoodCount;
    }

    private IEnumerator Cook()
    {
        Recipe recipe = RecipeManager.Instance.Search(cookingMethod, containFoods);
        float cookDuration = recipe.getCookTime();
        currProgressTime = 0;

        while (currProgressTime < cookDuration)
        {
            if(STOP)
                yield break;
            
            currProgressTime += 0.1f;
            Debug.Log($"Progress: {currProgressTime} / {cookDuration}%");
            yield return new WaitForSeconds(0.1f);
        }

        containFoods.Clear();
        cookedFood = Instantiate(recipe.getCookedFood().gameObject, transform.position + cookedOffset, Quaternion.identity);
        if (cookedFood == null)
        {
            Debug.Log("Invalid Component : 'Food'");
        }
        else cookwareState = ECookwareState.Complete;
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
        if(cookwareState != ECookwareState.Complete)
        {
            StartCook();
        }
    }

    protected abstract bool CheckFood(); // 해당 조리 도구에 들어갈 수 있는 음식인지 확인 

    //protected abstract bool CheckCook();

    public GameObject getCookedObject()
    {
        return cookedFood.gameObject;
    }

    public GameObject Get()
    {
        if (isGrabbable)
        {
            return this.gameObject;
        } else
        {
            return getCookedObject();
        }
    }

    public void Put(GameObject gameObject)
    {
        if(cookwareState == ECookwareState.Idle)
        {
            if (gameObject.tag == "Food")
            {
                if (CheckFood())
                {
                    if (!Full())
                    {
                        containFoods.Add(gameObject.GetComponent<Food>());
                        if (IsImmediateCook && Full())
                        {
                            StartCook();
                        }
                    }
                }
            }
        }
    }
}
