using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Unit
{
    [SerializeField]
    protected Vector3 _handPos;
    [SerializeField]
    public IObject _hand;

    [SerializeField]
    protected float distance;

    public void Grab(IObject ob) 
    { 
        _hand = ob;
        _hand.gameObject.layer = LayerMask.NameToLayer("Hand");
    }

    public void Put()
    {
        if (_hand != null)
            _hand.gameObject.layer = LayerMask.NameToLayer("Interactable");
        _hand = null;
    }
}
