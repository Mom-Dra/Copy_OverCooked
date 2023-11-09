using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FoodTray : CombinableTray, IFood
{
    [SerializeField]
    private bool isCookable = true;
    [SerializeField]
    private EFoodState foodState;
    [SerializeField]
    private ECookingMethod cookingMethod;

    private float currCookingRate = 0;
    private int currOverTime = 0;

    private Dish dish;

    private GameObject cookingPrefab;
    private GameObject cookedPrefab;
    private GameObject activePrefab;

    // Property
    public bool IsCookable
    {
        get => isCookable;
    }

    public EFoodState FoodState
    {
        get => foodState;
    }

    public ECookingMethod CookingMethod
    {
        get => cookingMethod;
    }

    public override List<EObjectSerialCode> Ingredients
    {
        get
        {
            // Memory_Optimizing
            List<EObjectSerialCode> tmp = new List<EObjectSerialCode> { serialCode };
            if (dish.Ingredients.Count > 0)
            {
                tmp.AddRange(dish.Ingredients);
            }
            return tmp;
        }
    }

    public float CurrCookingRate
    {
        get
        { 
            return currCookingRate; 
        }
        set
        {
            currCookingRate = value;
        }
    }

    public int CurrOverTime 
    { 
        get
        {
            return currOverTime;
        }
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
        

        if (foodState == EFoodState.Prepped)
        {
            uIComponent.Add(serialCode);
        }

        cookingPrefab = transform.Find("Cooking")?.gameObject;
        cookedPrefab = transform.Find("Cooked")?.gameObject;
    }

    protected override void Start()
    {
        base.Start();
        GetObject = dish = GetComponentInChildren<Dish>(true);
        dish.Init();
        //dish.Combine(this);
    }

    public override EObjectType GetTopType()
    {
        return EObjectType.Food;
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (!interactableObject.GetComponent<FoodTray>() && interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            if (iFood.FoodState != EFoodState.Burned && iFood.FoodState == EFoodState.Cooked)
            {
                return base.IsValidObject(interactableObject);
                //return TryCheckRecipe(ECookingMethod.Combine, iFood, out Recipe recipe);
            }
        }
        return false; 
    }

    public override void Put(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent(out IFood iFood))
        {
            CombineToDish(iFood);
        }
    }

    protected override void OnDestroy()
    {
        uIComponent.Clear();
    }

    public void OnBurned()
    {

    }

    public void OnPlated()
    {

    }

    public void OnCooking()
    {
        foodState = EFoodState.Cooking;
        if (cookingPrefab != null)
        {
            activePrefab?.gameObject.SetActive(false);
            cookingPrefab.gameObject.SetActive(true);
            activePrefab = cookingPrefab;
        } 
        else if (cookedPrefab != null)
        {
            activePrefab?.gameObject.SetActive(false);
            cookedPrefab.gameObject.SetActive(true);
            activePrefab = cookedPrefab;
        }
    }

    public void OnCooked()
    {
        foodState = EFoodState.Cooked;
        if (cookedPrefab != null && !cookedPrefab.activeSelf)
        {
            activePrefab?.gameObject.SetActive(false);
            cookedPrefab.gameObject.SetActive(true);
            activePrefab = cookedPrefab;
        }
    }
}