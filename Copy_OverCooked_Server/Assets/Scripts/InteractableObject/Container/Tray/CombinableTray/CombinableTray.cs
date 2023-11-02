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
        if (interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            if (iFood.FoodState == EFoodState.Burned || iFood.FoodState == EFoodState.Cooking)
                return false;

            if (!HasObject())
                return true;

            List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
            tmp.AddRange(iFood.Ingredients);

            if (getObject.TryGet<Dish>(out Dish dish))
            {
                if (interactableObject.GetComponent<Dish>())
                {
                    return false;
                }
                return dish.IsValidIngredients(tmp);
            } 
            else
            {
                tmp.AddRange(Ingredients);

                return DishManager.Instance.TryGetDish(tmp, out Dish result);
            }
        }
        return false;
    }

    public void CombineToDish(IFood iFood)
    {
        if (getObject.TryGet<Dish>(out Dish dish))
        {
            dish.Combine(iFood.Ingredients);
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

                List<EObjectSerialCode> tmp2 = new List<EObjectSerialCode>();
                tmp2.AddRange(iFood.Ingredients);

                if(HasObject() && getObject.TryGetComponent<Food>(out Food getFood))
                {
                    tmp2.AddRange(getFood.Ingredients);

                    uIComponent.AddRange(getFood.Ingredients);
                    Destroy(getFood.GameObject);
                }

                GetObject = putDish;

                putDish.Combine(tmp2);
            }
        }
        uIComponent.AddRange(iFood.Ingredients);
        Destroy(iFood.GameObject);
    }
}