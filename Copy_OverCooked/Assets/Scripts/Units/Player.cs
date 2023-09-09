using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Unit
{
    [SerializeField]
    protected Vector3 _handPos;
    [SerializeField]
    public IObject hand;

    [SerializeField]
    protected float distance;

    public void Grab(IObject ob) 
    { 
        hand = ob;
        hand.gameObject.layer = LayerMask.NameToLayer("Hand");
    }

    public void Put()
    {
        if (hand != null)
            hand.gameObject.layer = LayerMask.NameToLayer("Interactable");
        hand = null;
    }
}
