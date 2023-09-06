using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CookManager : Singleton<CookManager>
{
    Recipe recipe = new Recipe();
    // 레시피

    // 조리법 

    int wayToCook;

    public GameObject Cooking(int id, CookingMethod cookingMethod)
    {
        Food food = recipe.search(id, cookingMethod);
        return null;
    }
}
