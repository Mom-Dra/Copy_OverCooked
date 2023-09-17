using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private static RecipeManager instance = null;
    [Header("Recipe DB")]
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

    public Recipe Search(CookingMethod cookingMethod, List<InteractableObject> objects)
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            if (recipes[i].Equal(cookingMethod, objects))
            {
                return recipes[i];
            }
        }
        return null;
    }
}