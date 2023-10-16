using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Food : InteractableObject
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

    private UIComponent uIComponent;

    // Property
    public EFoodState FoodState 
    { 
        get => foodState; 
    }

    public List<EObjectSerialCode> Ingredients
    {
        get => ingredients;
    }

    public UIComponent UIComponent
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
        uIComponent = new UIComponent(transform, UIOffset);
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

    public void AddUISelf()
    {
        foreach (EObjectSerialCode serialCode in ingredients)
        {
            uIComponent.Add(serialCode);
        }
    }

    private void OnDestroy()
    {
        ingredients.Clear();
        uIComponent.Clear();
    }

}