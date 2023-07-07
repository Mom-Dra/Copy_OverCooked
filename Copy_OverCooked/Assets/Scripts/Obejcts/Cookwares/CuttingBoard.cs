using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Cookware
{
    protected override void Idle(Player player)
    {
        if(player.hand != null && player.hand.tag == "Food"){
            PutIn(player);
            totalProgressTime = food.GetComponent<Food>().chopDuration;
        }
    }

    protected override void Progressing(Player player)
    {
        StartCoroutine(Keep(player, food.GetComponent<Food>().chopped));
    }

    protected override void Completed(Player player)
    {
        TakeOut(player);
    }
}
