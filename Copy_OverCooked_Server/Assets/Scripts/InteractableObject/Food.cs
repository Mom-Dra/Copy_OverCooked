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

    // ������ ���� �� ���� �� �ϳ��ۿ� �ȳ��ðŰ�����
    // ������ Ingredients �迭�� Tray���� �����ϴϱ�
    // Food ���ο������� ingredietns �迭�� ������ �ʿ䰡 �����Ű�����.... (���� ����)
    // �ϴ� ���� 
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
        // PrevPosition �����ؾߵ�!

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