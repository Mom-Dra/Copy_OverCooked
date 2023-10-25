using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
#pragma warning disable CS0659 // ������ Object.Equals(object o)�� ������������ Object.GetHashCode()�� ���������� �ʽ��ϴ�.
public class Recipe : ScriptableObject
#pragma warning restore CS0659 // ������ Object.Equals(object o)�� ������������ Object.GetHashCode()�� ���������� �ʽ��ϴ�.
{
    [Header("Incomes")]
    [SerializeField]
    private ECookingMethod cookingMethod;

    [SerializeField]
    private List<EObjectSerialCode> ingredients;

    [SerializeField]
    private float totalCookDuration;

    [Header("OutComes")]
    [SerializeField]
    private EObjectSerialCode cookedFood;

    // Property
    public int IngredientCount
    {
        get => ingredients.Count;
    }
    public EObjectSerialCode CookedFood { get => cookedFood; }
    public float TotalCookDuration { get => totalCookDuration; }

    [ContextMenu("Set Recipe")]
    private void SetRecipeIngredients()
    {
        cookedFood = (EObjectSerialCode)Enum.Parse(typeof(EObjectSerialCode), name);
        string[] split = name.Split('_');
        ingredients.Clear();
        ingredients.Add((EObjectSerialCode)Enum.Parse(typeof(EObjectSerialCode), split[1]));
    }

    public bool Equal(ECookingMethod cookingMethod, List<EObjectSerialCode> foods)
    {
        if (this.cookingMethod == cookingMethod)
        {
            if (this.ingredients.Count == foods.Count)
            {
                if (this.ingredients.OrderBy(e => e).SequenceEqual(foods.OrderBy(e => e)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool Equal(ECookingMethod cookingMethod, EObjectSerialCode cookedFood)
    {
        if (this.cookingMethod == cookingMethod)
        {
            if (this.cookedFood == cookedFood)
            {
                return true;
            }
        }
        return false;
    }
}
