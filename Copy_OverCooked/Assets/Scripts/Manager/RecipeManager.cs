using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField]
    private Recipe[] recipes;

    public string Search(Recipe recipe)
    {
        for(int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i] == recipe)
            {
                return recipes[i].getCookedFood();
            }
        }
        return null;
    }
}