using System.Collections.Generic;
using UnityEngine;

public class DishCombine
{
    private List<EObjectSerialCode> currIngredients = new List<EObjectSerialCode>();
    private Dish dish = null;

    //public bool TryCombine(List<EObjectSerialCode> ingredients)
    //{
    //    if(dish == null)
    //    {
    //        DishManager.Instance.TryGetDish(ingredients, out dish);
    //        return dish != null;
    //    } 
    //    else
    //    {
    //        List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
    //        tmp.AddRange(currIngredients);
    //        tmp.AddRange(ingredients);
    //        return dish.IsValidIngredients(tmp);
    //    }
    //}

    //public void Combine(EObjectSerialCode ingredient)
    //{
    //    List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
    //    tmp.AddRange(currIngredients);
    //    tmp.Add(ingredient);
    //    dish.Put(tmp);
    //}
}