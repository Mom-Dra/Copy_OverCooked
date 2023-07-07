using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Unit
{
    [SerializeField]
    protected Vector3 _handPos;
    [SerializeField]
    public Object hand;
    [SerializeField]
    protected float Speed = 20f;
    [SerializeField]
    protected float distance = 1f;

    public void Hand(Object ob){
        if(ob == null){
            if(hand != null)
                hand.gameObject.layer = LayerMask.NameToLayer("Interactable");
            hand = null;
        }else{
            hand = ob;
            hand.gameObject.layer = LayerMask.NameToLayer("Hand");
        }
        
    }
}
