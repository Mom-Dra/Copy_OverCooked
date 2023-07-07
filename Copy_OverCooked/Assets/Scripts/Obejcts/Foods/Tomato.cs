using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Food, Interactable
{
    public void Interact(Player player)
    {
        Grab(player);
    }
}
