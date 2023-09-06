using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoodDictionary : MonoBehaviour
{
    [SerializeField]
    private static Dictionary<int, Food> dictionary = new Dictionary<int, Food>(500);

    private void Awake()
    {

    }

    public static Food search(int id)
    {
        return dictionary[id];
    }
}
