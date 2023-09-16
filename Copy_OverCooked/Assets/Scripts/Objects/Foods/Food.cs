using UnityEngine;

public class Food : InteractableObject
{
    //private bool isCooking = false;
    //private float currentCookTime = 0f;

    [Header("Cook")]
    [SerializeField]
    private CookingMethod cookingMethod;
}
