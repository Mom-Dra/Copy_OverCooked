using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected void Grab(Player player)
    {
        Debug.Log("Grab!");
        player.Hand(this);
    }
}
