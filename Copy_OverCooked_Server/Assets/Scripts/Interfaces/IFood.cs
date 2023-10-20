using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IFood
{
    public bool IsCookable
    {
        get;
    }
    public EFoodState FoodState
    {
        get;
    }

    public List<EObjectSerialCode> Ingredients
    {
        get;
    }

    public FoodUIComponent FoodUIComponent
    {
        get ;
    }

    public int CurrCookingRate
    {
        get;
        set;
    }

    public int CurrOverTime
    {
        get;
        set;
    }

    public GameObject GameObject
    {
        get;
    }

    public void OnBurned();
}