using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
#pragma warning disable CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
public class Recipe : ScriptableObject
#pragma warning restore CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
{
    [SerializeField]
    private CookingMethod cookingMethod;

    [SerializeField]
    private List<Food> foods;

    [SerializeField]
    private float cookedTime;

    [SerializeField]
    private Food cookedFood;

    public Recipe(CookingMethod cookingMethod, List<Food> foods)
    {
        this.cookingMethod = cookingMethod;
        this.foods = foods;
    }

    public Food getCookedFood()
    {
        return cookedFood;
    }

    public float getCookTime()
    {
        return cookedTime;
    }

    public override bool Equals(object other)
    {
        Recipe recipe = other as Recipe;

        if (cookingMethod == recipe.cookingMethod)
        {
            if (foods.Count == recipe.foods.Count)
            {
                if (foods.OrderBy(e => e).SequenceEqual(recipe.foods.OrderBy(e => e)))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
