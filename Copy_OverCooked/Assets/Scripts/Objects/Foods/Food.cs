using UnityEngine;

public class Food : InteractableObject
{
    public float currentCookTime;

    [Header("Food")]
    [SerializeField]
    private CookingMethod cookingMethod;

    public CookingMethod GetCookingMethod() 
    { 
        return cookingMethod; 
    }
}
