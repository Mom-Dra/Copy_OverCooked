using System;
using UnityEngine;

#pragma warning disable CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
public class Food : InteractableObject, IComparable<Food>
#pragma warning restore CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
{
    public int Id = 0;
    public FoodState _state;

    public int getId()
    {
        return Id;
    }

    public override bool Equals(object other)
    {
        Food food = other as Food;
        Debug.Log("Equal : " + food);
        return Id == food.Id && Name.Equals(food.Name);
    }

    public int CompareTo(Food other)
    {
        return Id - other.Id;
    }
}
