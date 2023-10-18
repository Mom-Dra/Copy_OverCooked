using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Food : InteractableObject, IFood, IFoodUIAttachable
{
    [Header("Food")]
    [SerializeField]
    private EFoodState foodState;

    [SerializeField]
    private List<EObjectSerialCode> ingredients = new List<EObjectSerialCode>();

    private int currCookingRate = 0;
    private int currOverTime = 0;

    private FoodUIComponent uIComponent;

    // Property
    public EFoodState FoodState 
    { 
        get => foodState; 
    }

    // 음식의 재료는 각 음식 당 하나밖에 안나올거같은데
    // 어차피 Ingredients 배열은 Tray에서 관리하니까
    // Food 내부에서까지 ingredietns 배열을 관리할 필요가 없을거같은데.... (추후 수정)
    // 일단 냅둠 
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

    public GameObject GameObject
    {
        get
        {
            return gameObject;
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
        // PrevPosition 도입해야됨!

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