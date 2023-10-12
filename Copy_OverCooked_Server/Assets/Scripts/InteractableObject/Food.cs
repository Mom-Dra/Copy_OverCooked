using UnityEngine;

public class Food : InteractableObject
{
    [Header("Food")]
    [SerializeField]
    private EFoodState foodState;
    [SerializeField]
    private int currCookDegree = 0;

    // Property
    public EFoodState FoodState 
    { 
        get => foodState; 
    }
    public int CurrCookDegree 
    { 
        get => currCookDegree; 
        set 
        { 
            currCookDegree = value; 
        } 
    }

    private void FixedUpdate()
    {
        if (UIComponent != null)
        {
            UIComponent.OnUIPositionChanging();
        }
    }

}