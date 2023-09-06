using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Cookware : Entity, Interactable
{
    [SerializeField]
    protected int maxElementCount;
    [SerializeField]
    private float offsetY = 0.1f;
    [SerializeField]
    private Vector2[] offsets2D;
    [SerializeField]
    protected Vector3 cookedOffset;


    protected Vector3[] offsets;
    protected CookwareState _state;
    protected List<Food> elements;
    // protected GameObject cookedPrefab;
    protected Food cooked;

    protected float totalProgressTime;
    protected float currProgressTime = 0;

    private void Awake()
    {
        _state = CookwareState.Idle;
        elements = new List<Food>(maxElementCount);
        offsets = new Vector3[maxElementCount];
        for (int i = 0; i < maxElementCount; ++i)
        {
            offsets[i] = new Vector3(offsets2D[i].x, offsetY, offsets2D[i].y);
        }
    }

    // protected void Reserve(Player player, float time)
    // {

    // }

    protected IEnumerator Cook(Player player, bool reserve = false)
    {
        while (currProgressTime < totalProgressTime)
        {
            RaycastHit hit;
            if (reserve || Physics.Raycast(transform.position, transform.forward, out hit, 1, LayerMask.GetMask("Player"))
            && hit.transform.GetComponent<Player>().hand == null)
            {
                currProgressTime += 0.1f;
                Debug.Log($"Progress: {currProgressTime} / {totalProgressTime}%");
                yield return new WaitForSeconds(0.1f);
            }
            else yield break;
        }
        DestroyAllElement();
        GameObject cookedPrefab = CookManager.Instance.Cooking(1, GetCookingMethod());
        cooked = Instantiate(cookedPrefab, transform.position + cookedOffset, Quaternion.identity).GetComponent<Food>();
        if (cooked == null)
        {
            Debug.Log("Invalid Component : 'Food'");
        }
        else _state = CookwareState.Completed;
    }

    protected void TakeOut(Player player)
    {
        Debug.Log("Take Out!");
        player.Hand(cooked);
        cooked = null;
        if (_state == CookwareState.Completed)
            _state = CookwareState.Idle;
    }

    protected void PutIn(Player player)
    {
        if (elements.Count == maxElementCount)
        {
            Debug.Log("Full Elements!");
            return;
        }
        Debug.Log("Put In!");
        Food food = player.hand.GetComponent<Food>();
        if (food == null) return;
        player.Hand(null);
        AddElement(food);
    }

    private void AddElement(Food food)
    {
        food.transform.position = transform.position + offsets[elements.Count];
        elements.Add(food);
    }

    private void RemoveElement(Food food)
    {
        if (elements.Count == 0) return;

        elements.Remove(food);
    }

    private void DestroyAllElement()
    {
        foreach (Food food in elements)
        {
            Destroy(food.gameObject);
        }
        elements.Clear();
    }

    public void Interact(Player player)
    {
        switch (_state)
        {
            case CookwareState.Idle:
                Idle(player);
                if (ProgressCondition())
                {
                    _state = CookwareState.Progressing;
                    Interact(player);
                }
                break;
            case CookwareState.Progressing:
                Progressing(player);
                break;
            case CookwareState.Completed:
                Completed(player);
                break;
        }
    }
    protected abstract void Idle(Player player);
    protected abstract bool ProgressCondition();
    protected abstract void Progressing(Player player);
    protected abstract void Completed(Player player);
    protected abstract CookingMethod GetCookingMethod();
}
