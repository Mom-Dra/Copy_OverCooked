using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : Entity, Interactable
{
    private int id;
    protected FoodState _state;
    [SerializeField]
    public float chopDuration;
    [SerializeField]
    public GameObject chopped;

    public void Interact(Player player)
    {
        Grab(player);
    }

    public int getId()
    {
        return id;
    }
}
