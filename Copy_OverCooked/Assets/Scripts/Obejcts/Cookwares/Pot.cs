using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Cookware, Interactable
{
    private void Awake() {
        _state = CookwareState.Empty;
    }
    public void Interact(Player player)
    {
        switch(_state){
            case CookwareState.Empty:
                if(player.hand == null){
                    Grab(player);
                }else{
                    PutIn(player);
                }
                _state = CookwareState.Progressing;
                break;
            case CookwareState.Progressing:
                break;
            case CookwareState.Completed:
                TakeOut(player);
                break;
        }
    }
}
