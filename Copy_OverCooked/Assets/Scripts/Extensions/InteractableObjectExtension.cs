using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class InteractableObjectExtension 
{
    public static EHandState GetEHandState(this InteractableObject gameObject)
    {
        switch (gameObject.GetObjectType())
        {
            case EObjectType.Food:
                return EHandState.Food;

            default: // Container
                return EHandState.Container;
        }
    }

    public static HandState GetHandState(this InteractableObject gameObject)
    {
        switch (GetEHandState(gameObject))
        {
            case EHandState.Food:
                return FoodHandState.Instance;

            case EHandState.Container:
                return ContainerHandState.Instance;

            default:
                return null;
        }
    }
}
