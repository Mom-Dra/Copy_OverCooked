using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Cookware, Interactable
{
    private void Awake() {
        _state = CookwareState.Empty;
    }
    public void Interact(Player player)
    {
        switch(_state){
            case CookwareState.Empty:
                if(player.hand != null && player.hand.tag == "Food"){
                    PutIn(player);
                    totalProgressTime = food.GetComponent<Food>().chopDuration;
                }
                _state = CookwareState.Progressing;
                break;
            case CookwareState.Progressing:
                StartCoroutine(Keep(player, food.GetComponent<Food>().chopped));
                break;
            case CookwareState.Completed:
                TakeOut(player);
                break;
        }
    }
}
