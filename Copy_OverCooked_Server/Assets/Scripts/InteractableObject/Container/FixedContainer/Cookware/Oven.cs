using UnityEngine;

public class Oven : Cookware
{
    protected override bool CanCook()
    {
        return true;
    }
}