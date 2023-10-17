using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Food : InteractableObject, IFoodUIAttachable
{
    [Header("Food")]
    [SerializeField]
    private EFoodState foodState;

    [SerializeField]
    private List<EObjectSerialCode> ingredients = new List<EObjectSerialCode>();

    [SerializeField]
    private int currCookingRate = 0;
    [SerializeField]
    private int currOverTime = 0;

    private FoodUIComponent uIComponent;

    // Property
    public EFoodState FoodState 
    { 
        get => foodState; 
    }

    public List<EObjectSerialCode> Ingredients
    {
        get => ingredients;
    }

    public FoodUIComponent FoodUIComponent
    {
        get => uIComponent;
    }

    public int CurrCookingRate
    {
        get => currCookingRate;
        set
        {
            currCookingRate = value;
        }
    }

    public int CurrOverTime 
    { 
        get => currOverTime; 
        set 
        {
            currOverTime = value; 
        } 
    }

    protected override void Awake()
    {
        base.Awake();
        uIComponent = new FoodUIComponent(transform, UIOffset);
        if(ingredients.Count == 0)
        {
            ingredients.Add(SerialCode);
        }
        //foreach(EObjectSerialCode serialCode in ingredients)
        //{
        //    uIComponent.Add(serialCode);
        //}
    }

    private void FixedUpdate()
    {
        // PrevPosition µµÀÔÇØ¾ßµÊ!

        if (uIComponent.HasImage)
        {
            uIComponent.OnImagePositionUpdate();
        }
    }

    public void OnBurned()
    {
        foodState = EFoodState.Burned;
        Renderer renderer = GetComponent<Renderer>();
        renderer?.material.SetColor("_Color", Color.black);
    }

    private void OnDestroy()
    {
        ingredients.Clear();
        uIComponent.Clear();
    }

    public void AddIngredientImages()
    {
        foreach (EObjectSerialCode serialCode in ingredients)
        {
            uIComponent.Add(serialCode);
        }
    }
}