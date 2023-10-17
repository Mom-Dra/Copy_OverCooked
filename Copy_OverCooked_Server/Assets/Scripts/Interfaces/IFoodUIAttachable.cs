using UnityEngine;

public interface IFoodUIAttachable
{
    public FoodUIComponent FoodUIComponent
    { 
        get; 
    }
    public void AddIngredientImages();
}