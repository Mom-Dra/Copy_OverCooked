using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
public class Food : InteractableObject, IComparable<Food>
#pragma warning restore CS0659 // 형식은 Object.Equals(object o)를 재정의하지만 Object.GetHashCode()를 재정의하지 않습니다.
{ 
    public int Id;
    public FoodState _state;

    public int getId()
    {
        return Id;
    }

    public override bool Equals(object other)
    {
        Food food = other as Food;
        return Id == food.Id && Name.Equals(food.Name);
    }

    public int CompareTo(Food other)
    {
        return Id - other.Id;
    }
}
