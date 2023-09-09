using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Cookware : IObject
{
    [SerializeField]
    protected int maxElementCount;
    [SerializeField]
    private float offsetY = 0.1f;
    [SerializeField]
    private Vector2[] offsets2D;
    [SerializeField]
    protected Vector3 cookedOffset;
    [SerializeField]
    protected CookingMethod cookingMethod;


    protected Vector3[] offsets;
    protected CookwareState _state;
    protected List<Food> foods;
    // protected GameObject cookedPrefab;
    protected Food cooked;

    protected Player player;

    

    private void Awake()
    {
        _state = CookwareState.Idle;
        foods = new List<Food>();
        offsets = new Vector3[maxElementCount];
        for (int i = 0; i < maxElementCount; ++i)
        {
            offsets[i] = new Vector3(offsets2D[i].x, offsetY, offsets2D[i].y);
        }
    }

    // protected void Reserve(Player player, float time)
    // {

    // }

    protected IEnumerator Cook(bool reserve = false)
    {
        Recipe recipe = RecipeManager.Instance.Search(cookingMethod, foods);
        float cookTime = recipe.getCookTime();
        float currProgressTime = 0;

        while (currProgressTime < cookTime)
        {
            RaycastHit hit;
            if (reserve || Physics.Raycast(transform.position, transform.forward, out hit, 1, LayerMask.GetMask("Player"))
            && hit.transform.GetComponent<Player>().hand == null)
            {
                currProgressTime += 0.1f;
                Debug.Log($"Progress: {currProgressTime} / {cookTime}%");
                yield return new WaitForSeconds(0.1f);
            }
            else yield break;
        }

        DestroyAllElement();
        cooked = Instantiate(recipe.getCookedFood().gameObject, transform.position + cookedOffset, Quaternion.identity).GetComponent<Food>();
        if (cooked == null)
        {
            Debug.Log("Invalid Component : 'Food'");
        }
        else _state = CookwareState.Completed;
    }

    protected void TakeOut()
    {
        Debug.Log("Take Out!");
        player.Grab(cooked);
        cooked = null;
        if (_state == CookwareState.Completed)
            _state = CookwareState.Idle;
    }

    protected void PutIn()
    {
        if (foods.Count == maxElementCount)
        {
            Debug.Log("Full Elements!");
            return;
        }
        Debug.Log("Put In!");
        Food food = player.hand.GetComponent<Food>();
        if (food == null) return;
        player.Put();
        AddElement(food);
    }

    private void AddElement(Food food)
    {
        food.transform.position = transform.position + offsets[foods.Count];
        foods.Add(food);
    }

    private void RemoveElement(Food food)
    {
        if (foods.Count == 0) return;

        foods.Remove(food);
    }

    private void DestroyAllElement()
    {
        foreach (Food food in foods)
        {
            Destroy(food.gameObject);
        }
        foods.Clear();
    }

    public void Interact(Player player)
    {
        this.player = player;
        switch (_state)
        {
            case CookwareState.Idle:
                Idle();
                if (ProgressCondition())
                {
                    _state = CookwareState.Progressing;
                    Interact(player);
                } 
                break;
            case CookwareState.Progressing:
                Progressing();
                break;
            case CookwareState.Completed:
                Completed();
                break;
        }
        this.player = null;
    }

    // Player 매개변수 이거 나중에 심플하게 바꿔보자
    // Cookware 안에 Player 변수 하나 두고
    // Player가 Cookware와 상호작용 하는 동안만 Player-Cookware 링크를 걸어두는 식
    
    protected abstract void Idle(); 
    protected abstract bool ProgressCondition();
    protected abstract void Progressing();
    protected abstract void Completed();
    protected abstract CookingMethod GetCookingMethod();
}
