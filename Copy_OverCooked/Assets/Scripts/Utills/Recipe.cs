using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    List<int> raws = new List<int>();
    List<int> chopRecipe = new List<int>();
    List<int> roastRecipe = new List<int>();
    List<int> boilRecipe = new List<int>();
    List<int> fryRecipe = new List<int>();

    private List<int> getRecipe(CookingMethod cookingMethod)
    {
        switch (cookingMethod)
        {
            case CookingMethod.Chop:
                return chopRecipe;
            case CookingMethod.Boil:
                return boilRecipe;
            case CookingMethod.Fry:
                return fryRecipe;
            case CookingMethod.Roast:
                return roastRecipe;
        }
        return null;
    }

    public void addRecipe(CookingMethod cookingMethod, Food food)
    {

    }

    public Food search(int id, CookingMethod cookingMethod)
    {
        List<int> recipe = getRecipe(cookingMethod);
        return FoodDictionary.search(recipe[id]);
    }







}
