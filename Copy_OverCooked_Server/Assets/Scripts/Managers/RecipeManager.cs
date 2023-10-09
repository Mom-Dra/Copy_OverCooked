using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private static RecipeManager instance;
    public static RecipeManager Instance
    {
        get => instance;
    }

    [SerializeField]
    private List<Recipe> recipes = new List<Recipe>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public bool TryGetRecipe(ECookingMethod eCookingMethod, List<InteractableObject> foods, out Recipe outRecipe)
    {
        outRecipe = default(Recipe);
        foreach(Recipe recipe in recipes)
        {
            if(recipe.Equal(eCookingMethod, foods))
            {
                outRecipe = recipe; 
                break;
            }
        }
        return outRecipe != null;
    }
}