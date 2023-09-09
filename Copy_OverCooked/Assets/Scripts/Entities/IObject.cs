using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IObject : MonoBehaviour
{
    public bool IsGrabable { get; protected set; }
    protected void Grab(Player player)
    {
        Debug.Log("Grab!");
        player.Hand(this);
    }
}
