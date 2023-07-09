using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Cookware
{
    protected override void Idle(Player player)
    {
        if (player.hand == null)
        {
            Grab(player);
        }
        else
        {
            PutIn(player);
        }
    }

    protected override bool ProgressCondition()
    {
        return elements.Count == maxElementCount;
    }

    protected override void Progressing(Player player)
    {
        StartCoroutine(Cook(player, true));
    }

    protected override void Completed(Player player)
    {
        TakeOut(player);
    }
}
