using OpenCover.Framework.Model;
using System.Security.Cryptography;
using UnityEngine;

public class FoodBox : FixedContainer
{
    [Header("Food Box")]
    [SerializeField]
    private Food food;

    public override bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek) 
    {
        if (!base.TryGet<T>(out result) && typeof(T) == typeof(Food))
        {
            result = Instantiate(food, transform.position + displayOffset, Quaternion.identity).GetComponent<T>();
        }
        return result != null;
    }
}