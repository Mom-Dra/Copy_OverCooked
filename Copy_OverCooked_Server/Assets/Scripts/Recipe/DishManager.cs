using System.Collections.Generic;
using UnityEngine;

public class DishManager : MonobehaviorSingleton<DishManager>
{
    private List<Dish> dishList = new List<Dish>();

    public bool TryGetDish(List<EObjectSerialCode> ingredients, out Dish result)
    {
        result = default(Dish);
        foreach (Dish dish in dishList)
        {
            if (dish.IsValidRecipe(ingredients))
            {
                result = Instantiate(dish);
                break;
            }
        }
        return result != null;
    }
}