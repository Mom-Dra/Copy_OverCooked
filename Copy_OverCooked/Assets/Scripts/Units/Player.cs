using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Unit
{
    [SerializeField]
    protected Vector3 _handPos;
    [SerializeField]
    public InteractableObject _hand;

}
