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

    public ECookingMethod CookingMethod
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

    public float CurrCookingRate
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
    public void OnPlated();
    public void OnCooking();
    public void OnCooked();
}