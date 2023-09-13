using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private static RecipeManager instance = null;
    [SerializeField]
    private Recipe[] recipes;

    public static RecipeManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    public Recipe Search(CookingMethod cookingMethod, List<Food> foods)
    {
        Recipe recipe = new Recipe(cookingMethod, foods);
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i].Equals(recipe))
            {
                return recipes[i];
            }
        }
        return null;
    }
}