using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Food : InteractableObject
{
    [Header("Food")]
    [SerializeField]
    private EFoodState foodState;

    [SerializeField]
    private int currCookingRate = 0;
    [SerializeField]
    private int currOverTime = 0;

    // Property
    public EFoodState FoodState 
    { 
        get => foodState; 
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

    private void FixedUpdate()
    {
        if (uIComponent.HasImage)
        {
            uIComponent.OnImagePositionUpdate();
        }
    }

    //public Image GetFoodImage()
    //{
    //    return SerialCodeDictionary.Instance.FindFoodImageSerialCode(serialCode);
    //}

}