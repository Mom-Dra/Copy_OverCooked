using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonobehaviorSingleton<RecipeManager>
{
    [SerializeField]
    private List<Recipe> recipes = new List<Recipe>();

    public bool TryGetRecipe(ECookingMethod eCookingMethod, List<Food> foods, out Recipe outRecipe)
    {
        outRecipe = default(Recipe);
        foreach (Recipe recipe in recipes)
        {
            if (recipe.Equal(eCookingMethod, foods))
            {
                outRecipe = recipe;
                break;
            }
        }
        return outRecipe != null;
    }
}