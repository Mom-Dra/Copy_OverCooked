using UnityEngine;

public class Plate : Container
{
    public override bool IsValidObject(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<Food>(out Food food))
        {
            if (food.foodState != EFoodState.Original)
            {
                return true;
            }
        }
        Debug.Log($"{this.Name} Invalid Object : Only Food has been Contained");
        return false;
    }



}
