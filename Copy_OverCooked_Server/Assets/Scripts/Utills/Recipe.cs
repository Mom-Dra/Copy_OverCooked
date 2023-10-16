using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
#pragma warning disable CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
public class Recipe : ScriptableObject
#pragma warning restore CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
{
    [Header("Incomes")]
    [SerializeField]
    private ECookingMethod cookingMethod;

    [SerializeField]
    private List<EObjectSerialCode> ingredients;

    [SerializeField]
    private float totalCookDuration;

    [SerializeField]
    private Image mainImage;

    [Header("OutComes")]
    [SerializeField]
    private Food cookedFood;

    // Property
    public Food CookedFood { get => cookedFood; }
    public float TotalCookDuration { get => totalCookDuration; }
    public Image MainImage
    {
        get => mainImage;
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
}
