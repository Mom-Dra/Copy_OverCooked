using UnityEngine;

public class Food : InteractableObject
{
    [Header("Food")]
    public EFoodState foodState;

    public float currCookTime;
}