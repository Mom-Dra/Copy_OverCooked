using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour
{
    [SerializeField]
    public GameObject prefab;

    protected void Grab(Player player){
        Debug.Log("Grab!");
        player.Hand(this);
    }
}
