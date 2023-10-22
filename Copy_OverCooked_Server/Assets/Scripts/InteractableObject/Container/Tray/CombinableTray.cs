using System.Collections.Generic;
using UnityEngine;

public class CombinableTray : Tray
{
    //protected DishCombine dishCombine = new DishCombine();
    //protected Dish tmpDish;

    protected void PutAndCombine(IFood iFood)
    {
        Test(iFood);
        //if (TryCheckRecipe(ECookingMethod.Combine, iFood, out Recipe recipe))
        //{
        //    // Plate만의 Prefab 까지 고려할 수 있게 코드 짜야함 
        //    Food combinedFood = Instantiate(SerialCodeDictionary.Instance.FindBySerialCode(recipe.CookedFood).GetComponent<Food>());

        //    if (HasObject() && !getObject.GetComponent<FoodTray>())
        //    {
        //        Destroy(getObject.gameObject);
        //        getObject = null;
        //    }

        //    // FoodTray가 Put 되었을 경우,
        //    // FoodTray에 FoodTray가 들어오는 경우의 수는 이미 막아놓았다. 
        //    if (iFood.GameObject.TryGetComponent<FoodTray>(out FoodTray foodTray2))
        //    {
        //        base.Put(foodTray2);

        //        uIComponent.AddRange(foodTray2.Ingredients);

        //        foodTray2.GetObject = combinedFood;
        //    } else
        //    {
        //        if (HasObject() && getObject.TryGetComponent<FoodTray>(out FoodTray foodTray3))
        //        {
        //            foodTray3.GetObject = combinedFood;
        //        } else
        //        {
        //            GetObject = combinedFood;
        //        }
        //        Destroy(iFood.GameObject);
        //        uIComponent.AddRange(iFood.Ingredients);
        //    }
        //}
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
            tmp.AddRange(iFood.Ingredients);
            if (getObject.TryGetComponent<Dish>(out Dish dish))
            {
                return dish.IsValidIngredients(iFood); 
            } 
            else
            {
                tmp.AddRange(Ingredients);
                return DishManager.Instance.TryGetDish(tmp, out Dish result);
            }
        }
        return false;
    }

    // PutAndCombine() 함수의 추후 대체 함수 
    public void Test(IFood iFood)
    {
        if(getObject.TryGetComponent<Dish>(out Dish dish))
        {
            dish.Combine(iFood);
        } 
        else
        {
            Destroy(getObject.gameObject);
            getObject = null;

            List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
            tmp.AddRange(iFood.Ingredients);
            tmp.AddRange(Ingredients);
            if (DishManager.Instance.TryGetDish(tmp, out Dish result))
            {
                GetObject = result;
                result.Combine(iFood);
            }
        }
        
        
    }
}