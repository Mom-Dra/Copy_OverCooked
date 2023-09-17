using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
#pragma warning disable CS0659 // ������ Object.Equals(object o)�� ������������ Object.GetHashCode()�� ���������� �ʽ��ϴ�.
public class Recipe : ScriptableObject
#pragma warning restore CS0659 // ������ Object.Equals(object o)�� ������������ Object.GetHashCode()�� ���������� �ʽ��ϴ�.
{
    [SerializeField]
    private CookingMethod cookingMethod;

    [SerializeField]
    private List<Food> foods;

    [SerializeField]
    private float totalCookDuration;

    [SerializeField]
    private Food cookedFood;

    public Food getCookedFood()
    {
        return cookedFood;
    }

    public float getTotalCookDuration()
    {
        return totalCookDuration;
    }

    public bool Equal(CookingMethod cookingMethod, List<InteractableObject> foods)
    {

        if (this.cookingMethod == cookingMethod)
        {
            if (this.foods.Count == foods.Count)
            {
                if (this.foods.OrderBy(e => e).SequenceEqual(foods.OrderBy(e => e.GetComponent<Food>())))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
