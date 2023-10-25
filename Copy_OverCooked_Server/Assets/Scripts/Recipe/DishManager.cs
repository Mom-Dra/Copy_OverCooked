using System.Collections.Generic;
using UnityEngine;

public class DishManager : MonobehaviorSingleton<DishManager>
{
    private List<Dish> dishList = new List<Dish>();

    protected override void Awake()
    {
        base.Awake();

        Dish[] _loadPrefab = Resources.LoadAll<Dish>("Prefabs/Dish");
        dishList.AddRange( _loadPrefab );

        foreach (Dish dish in dishList)
        {
            dish.Init();
        }
    }

    public bool TryGetDish(List<EObjectSerialCode> ingredients, out Dish result)
    {
        result = default(Dish);
        foreach (Dish dish in dishList)
        {
            if (dish.IsValidRecipe(ingredients))
            {
                result = dish;
                //result = Instantiate(dish);
                //result.Init();
                break;
            }
        }
        return result != null;
    }
}