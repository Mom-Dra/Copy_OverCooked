using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Food : InteractableObject, IFood, IFoodUIAttachable
{
    [Header("Food")]
    [SerializeField]
    protected bool isCookable = true;
    [SerializeField]
    protected EFoodState foodState;
    [SerializeField]
    protected ECookingMethod cookingMethod;

    [SerializeField]
    protected List<EObjectSerialCode> ingredients = new List<EObjectSerialCode>();

    [SerializeField]
    protected float currCookingRate = 0;
    protected int currOverTime = 0;

    protected FoodUIComponent uIComponent;

    protected GameObject originPrefab;
    protected GameObject platedPrefab;

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

    public virtual List<EObjectSerialCode> Ingredients
    {
        get => ingredients;
    }

    public FoodUIComponent FoodUIComponent
    {
        get => uIComponent;
    }

    public float CurrCookingRate
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

    public GameObject PlatedPrefab
    {
        get => platedPrefab;
    }

    protected override void Awake()
    {
        base.Awake();
        objectType = EObjectType.Food;
        uIComponent = new FoodUIComponent(transform, UIOffset);
        if(ingredients.Count == 0)
        {
            ingredients.Add(SerialCode);
        }

        if(foodState == EFoodState.Prepped)
        {
            uIComponent.Add(serialCode);
        }

        originPrefab = transform.Find("Origin")?.gameObject;
        platedPrefab = transform.Find("Plated")?.gameObject;
        cookingPrefab = transform.Find("Cooking")?.gameObject;
        cookedPrefab = transform.Find("Cooked")?.gameObject;

        platedPrefab?.gameObject.SetActive(false);
        cookingPrefab?.gameObject.SetActive(false);
        cookedPrefab?.gameObject.SetActive(false);

        activePrefab = originPrefab;
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
        Debug.Log($"Burn : {name}");
        if (uIComponent.HasImage)
        {
            uIComponent.Clear();
        }
        uIComponent.Add(EObjectSerialCode.Img_Overheat);
        //Renderer renderer = GetComponent<Renderer>();
        //renderer?.material?.SetColor("_Color", Color.black);
    }

    public virtual void OnPlated()
    {
        if (platedPrefab != null)
        {
            activePrefab?.gameObject.SetActive(false);
            platedPrefab?.gameObject.SetActive(true);
            activePrefab = platedPrefab;
        }
    }

    public void OnCooking()
    {
        foodState = EFoodState.Cooking;
        if(cookingPrefab != null)
        {
            activePrefab?.gameObject.SetActive(false);
            cookingPrefab.gameObject.SetActive(true);
            activePrefab = cookingPrefab;
        }else if(cookedPrefab != null)
        {
            activePrefab?.gameObject.SetActive(false);
            cookedPrefab.gameObject.SetActive(true);
            activePrefab = cookedPrefab;
        }
    }

    public void OnCooked()
    {
        foodState = EFoodState.Cooked;
        if (cookedPrefab != null)
        {
            if (!cookedPrefab.activeSelf)
            {
                activePrefab?.gameObject.SetActive(false);
                cookedPrefab.gameObject.SetActive(true);
                activePrefab = cookedPrefab;
            }
        }
    }

    public void AddIngredientImages()
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