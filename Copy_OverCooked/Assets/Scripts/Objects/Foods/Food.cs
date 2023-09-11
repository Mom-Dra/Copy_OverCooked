using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0659 // ������ Object.Equals(object o)�� ������������ Object.GetHashCode()�� ���������� �ʽ��ϴ�.
public class Food : InteractableObject, IComparable<Food>
#pragma warning restore CS0659 // ������ Object.Equals(object o)�� ������������ Object.GetHashCode()�� ���������� �ʽ��ϴ�.
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
