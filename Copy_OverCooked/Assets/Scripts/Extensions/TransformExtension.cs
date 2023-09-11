using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameObjectExtension 
{
    public static EHoldState? GetEHandState(this GameObject gameObject)
    {
        switch (gameObject.tag)
        {
            case "Food":
                return EHoldState.Food;

            case "Cookware":
                return EHoldState.Container;

            default:
                return null;
        }
    }

    public static HoldState GetHandState(this GameObject gameObject)
    {
        switch (GetEHandState(gameObject))
        {
            case EHoldState.Food:
                return HoldFoodState.Instance;

            case EHoldState.Container:
                return HoldContainerState.Instance;

            default:
                return null;
        }
    }
}
