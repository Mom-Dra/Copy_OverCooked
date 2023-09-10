using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Cookware
{
    protected override void Idle()
    {
        if (player._hand != null && player._hand.tag == "Food")
        {
            PutIn();
            //totalProgressTime = foods[0].GetComponent<Food>().chopDuration;
        }
    }

    protected override bool ProgressCondition()
    {
        return foods.Count == maxElementCount;
    }

    protected override void Progressing()
    {
        StartCoroutine(Cook());
    }

    protected override void Completed()
    {
        TakeOut();
    }

    protected override CookingMethod GetCookingMethod()
    {
        return CookingMethod.Chop;
    }

    public override GameObject GetObject()
    {
        GameObject go = cookedFood.gameObject;
        cookedFood = null;
        return go;
    }
}
