using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject, IComparable<Recipe>
{
    [SerializeField]
    private CookingMethod cookingMethod;

    [SerializeField]
    private List<string> foods = new List<string>();

    [SerializeField]
    private string cookedFood;

    public string getCookedFood()
    {
        return cookedFood;
    }

    public int CompareTo(Recipe other)
    {
        if(cookingMethod == other.cookingMethod)
        {
            if(foods.Count == other.foods.Count)
            {
                if(Enumerable.SequenceEqual(foods, other.foods)) return 0;
            }
        }
        return 1;
    }
}
