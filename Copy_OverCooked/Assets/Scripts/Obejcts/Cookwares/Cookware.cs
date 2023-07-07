using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cookware : Object
{
    
    [SerializeField]
    protected Vector3 offset;

    protected CookwareState _state;
    protected Object food;

    protected float totalProgressTime;
    protected float currProgressTime = 0;
    protected void Reserve(Player player, float time){
        
    }

    protected IEnumerator Keep(Player player, GameObject cookedPrefab){
        while(currProgressTime < totalProgressTime){
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Player");
            if(Physics.Raycast(transform.position, transform.forward, out hit, 1, layerMask)
            && hit.transform.GetComponent<Player>().hand == null){
                currProgressTime += 0.1f;
                Debug.Log($"Progress: {currProgressTime} / {totalProgressTime}%");
                yield return new WaitForSeconds(0.1f);
            }else yield break;
        }
        Destroy(food.gameObject);
        food = Instantiate(cookedPrefab, transform.position + offset, Quaternion.identity).GetComponent<Object>();
        _state = CookwareState.Completed;
    }

    protected void TakeOut(Player player){
        Debug.Log("Take Out!");
        player.Hand(food);
        ChangeFood(null);
    }

    protected void PutIn(Player player){
        Debug.Log("Put In!");
        Object ob = player.hand;
        player.Hand(null);
        ChangeFood(ob);
    }

    private void ChangeFood(Object ob){
        food = ob;
        if(food != null){
            food.transform.position = transform.position + offset;
        }
    }



}
