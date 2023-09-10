using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Cookware
{
    protected override void Idle()
    {
            PutIn();
    }

    protected override bool ProgressCondition()
    {
        return foods.Count == maxElementCount;
    }

    protected override void Progressing()
    {
        StartCoroutine(Cook(true));
    }

    protected override void Completed()
    {
        TakeOut();
    }

    protected override CookingMethod GetCookingMethod()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject GetObject()
    {
        throw new System.NotImplementedException();
    }
}
