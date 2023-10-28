using System.Collections.Generic;
using UnityEngine;

public class CombinableTray : Tray, IFoodUIAttachable
{
    protected override void Awake()
    {
        base.Awake();
        uIComponent = new FoodUIComponent(transform, uIOffset);
    }

    private void FixedUpdate()
    {
        if (uIComponent != null && uIComponent.HasImage)
        {
            uIComponent.OnImagePositionUpdate();
        }
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if (!HasObject())
            return true;

        if (interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
            tmp.AddRange(iFood.Ingredients);
            if (getObject.TryGet<Dish>(out Dish dish))
            {
                return dish.IsValidIngredients(iFood);
            } 
            else
            {
                tmp.AddRange(Ingredients);
                Debug.Log(DishManager.Instance.TryGetDish(tmp, out Dish resdsult));
                return DishManager.Instance.TryGetDish(tmp, out Dish result);
            }
        }
        return false;
    }

    public void PutDish(IFood iFood)
    {
        if (getObject.TryGet<Dish>(out Dish dish))
        {
            dish.Combine(iFood);
        } 
        else
        {
            // Memory_Optimizing
            List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
            tmp.AddRange(iFood.Ingredients);
            tmp.AddRange(Ingredients);

            if (DishManager.Instance.TryGetDish(tmp, out dish))
            {
                uIComponent.Clear();
                Dish putDish = Instantiate(dish);
                putDish.Init();
                if(getObject.TryGetComponent<IFood>(out IFood getIFood))
                {
                    putDish.Combine(getIFood);
                    uIComponent.AddRange(getIFood.Ingredients);
                }

                GetObject = putDish;

                putDish.Combine(iFood);
            }
        }
        uIComponent.AddRange(iFood.Ingredients);
    }
}