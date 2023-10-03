using OpenCover.Framework.Model;
using System.Security.Cryptography;
using UnityEngine;

public class FoodBox : FixedContainer
{
    [Header("Food Box")]
    [SerializeField]
    private Food food;

    public override bool TryGet<T>(out T result) 
    {
        if (!base.TryGet<T>(out result) && food is T)
        {
            result = Instantiate(food, transform.position + containOffset, Quaternion.identity).GetComponent<T>();
        }
        return result != null;
    }



}