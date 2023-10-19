using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodTray : Tray, IFood
{
    [SerializeField]
    private EFoodState foodState;

    private int currCookingRate = 0;
    private int currOverTime = 0;

    [SerializeField]
    private List<Food> foods = new List<Food>();

    public EFoodState FoodState
    {
        get => foodState;
    }

    public override InteractableObject GetObject
    {
        [Obsolete]
        get
        {
            return getObject;
        }
        set
        {
            //base.GetObject = value;
            if(value.TryGetComponent<Food>(out Food food))
            {
                foods.Add(food);
            }
            Fit(food);
        }
    }

    public override List<EObjectSerialCode> Ingredients
    {
        get
        {
            // Memory_Optimizing
            List<EObjectSerialCode> tmp = new List<EObjectSerialCode> { SerialCode };
            foreach (Food food in foods)
            {
                tmp.AddRange(food.Ingredients);
            }
            return tmp;
        }
    }

    public int CurrCookingRate
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(foods.Count > 0)
        {
            foreach(Food food in foods)
            {
                food.transform.position = transform.position + displayOffset;
            }
        }
    }

    public override EObjectType GetTopType()
    {
        return EObjectType.Food;
    }

    public void OnBurned()
    {
        
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !interactableObject.GetComponent<FoodTray>() && base.IsValidObject(interactableObject);
    }

    public override void Put(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            PutByRecipe(iFood);
        }
        //if (interactableObject.TryGetComponent<IFood>(out IFood putIFood))
        //{
        //    // Memory_Optimizing
        //    List<EObjectSerialCode> totalFoods = new List<EObjectSerialCode>();
        //    totalFoods.AddRange(putIFood.Ingredients);
        //    totalFoods.AddRange(Ingredients);

        //    if (TryGetCombinedRecipe(totalFoods, out Recipe recipe))
        //    {
        //        // Plate만의 Prefab 까지 고려할 수 있게 코드 짜야함 
        //        Food combinedFood = Instantiate(SerialCodeDictionary.Instance.FindBySerialCode(recipe.CookedFood).GetComponent<Food>());

        //        GetObject = combinedFood;

        //        uIComponent.AddRange(putIFood.Ingredients);

        //        Destroy(putIFood.GameObject);
        //    }
        //}
    }

    protected override void OnDestroy()
    {
        //base.OnDestroy();
        foreach(Food food in foods)
        {
            Destroy(food.gameObject);
        }
        foods.Clear();
        uIComponent.Clear();
    }
}