using UnityEngine;

public class Food : InteractableObject
{
    public float currentCookTime;


    [Header("Food")]
    public EFoodState foodState = EFoodState.Original;
    public ECookingMethod cookingMethod;

}
