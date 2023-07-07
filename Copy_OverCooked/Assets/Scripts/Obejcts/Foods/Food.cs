using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : Object, Interactable
{
    protected FoodState _state;
    [SerializeField]
    public float chopDuration;
    [SerializeField]
    public GameObject chopped;

    public abstract void Interact(Player player);
}
