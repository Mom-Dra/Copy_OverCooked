using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Cookware
{
    protected override void Idle(Player player)
    {
        if (player.hand != null && player.hand.tag == "Food")
        {
            PutIn(player);
            totalProgressTime = elements[0].GetComponent<Food>().chopDuration;
        }
    }

    protected override bool ProgressCondition()
    {
        return elements.Count == maxElementCount;
    }

    protected override void Progressing(Player player)
    {
        StartCoroutine(Cook(player));
    }

    protected override void Completed(Player player)
    {
        TakeOut(player);
    }
}
