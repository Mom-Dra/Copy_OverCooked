using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Cookware
{
    protected override void Idle(Player player)
    {
        if(player.hand == null){
            Grab(player);
        }else{
            PutIn(player);
        }
        _state = CookwareState.Progressing;
    }

    protected override void Progressing(Player player)
    {
        
    }

    protected override void Completed(Player player)
    {
        TakeOut(player);
    }
}
