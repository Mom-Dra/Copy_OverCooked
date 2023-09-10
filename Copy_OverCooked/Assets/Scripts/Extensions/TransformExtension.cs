using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameObjectExtension 
{
    public static EHandState? GetEHandState(this GameObject gameObject)
    {
        switch (gameObject.tag)
        {
            case "Food":
                return EHandState.Food;

            case "Cookware":
                return EHandState.Cookware;

            case "Plate":
                return EHandState.Plate;

            default:
                return null;
        }
    }

    public static HandState GetHandState(this GameObject gameObject)
    {
        switch (GetEHandState(gameObject))
        {
            case EHandState.Food:
                return FoodHandState.Instance;

            case EHandState.Cookware:
                return CookwareHandState.Instance;

            case EHandState.Plate:
                return PlateHandState.Instance;

            default:
                return null;
        }
    }
}
