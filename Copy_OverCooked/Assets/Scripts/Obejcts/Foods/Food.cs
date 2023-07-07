using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : Object
{
    protected FoodState _state;
    [SerializeField]
    public float chopDuration;
    [SerializeField]
    public GameObject chopped;
    
}
