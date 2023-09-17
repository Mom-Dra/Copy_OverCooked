using UnityEngine;

public class Food : InteractableObject
{
    private bool isCooking = false;
    public float currentCookTime;

    [Header("Cook")]
    [SerializeField]
    private CookingMethod cookingMethod;
}
