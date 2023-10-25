using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RecipeManager : MonobehaviorSingleton<RecipeManager>
{
    [SerializeField]
    private List<Recipe> recipes = new List<Recipe>();

    protected override void Awake()
    {
        base.Awake();
        Recipe[] load_Recipe = Resources.LoadAll<Recipe>("Prefabs/Recipes");
        recipes.AddRange(load_Recipe);
    }

    public bool TryGetRecipe(ECookingMethod eCookingMethod, List<EObjectSerialCode> foods, out Recipe outRecipe)
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

    public bool FindCookedFood(ECookingMethod cookingMethod, EObjectSerialCode cookedFood)
    {
        foreach (Recipe recipe in recipes)
        {
            if (recipe.Equal(cookingMethod, cookedFood))
            {
                return true;
            }
        }
        return false;
    }
}