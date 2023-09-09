using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : IObject, Interactable
{
    Food _food;

    public void Interact(Player player)
    {
        if (player.hand == null) return;
        Food food = player.hand.GetComponent<Food>();
        if (food == null) return;
    }
}
