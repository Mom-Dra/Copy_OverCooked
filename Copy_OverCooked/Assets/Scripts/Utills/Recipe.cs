using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
#pragma warning disable CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
public class Recipe : ScriptableObject
#pragma warning restore CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
{
    //[Header("Incomes")]
    //[SerializeField]
    //private ECookingMethod cookingMethod;

    //[SerializeField]
    //private List<Food> materialFoods;

    //[SerializeField]
    //private float totalCookDuration;

    //[Header("OutComes")]
    //[SerializeField]
    //private List<Food> extraFoods;

    //[SerializeField]
    //private Food cookedFood;

    //public List<Food> getExtraFoods()
    //{
    //    return extraFoods;
    //}

    //public Food getCookedFood()
    //{
    //    return cookedFood;
    //}

    //public float getTotalCookDuration()
    //{
    //    return totalCookDuration;
    //}

    //public bool Equal(ECookingMethod cookingMethod, List<InteractableObject> foods)
    //{

    //    if (this.cookingMethod == cookingMethod)
    //    {
    //        if (this.materialFoods.Count == foods.Count)
    //        {
    //            if (this.materialFoods.OrderBy(e => e).SequenceEqual(foods.OrderBy(e => e.GetComponent<Food>())))
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}
}
