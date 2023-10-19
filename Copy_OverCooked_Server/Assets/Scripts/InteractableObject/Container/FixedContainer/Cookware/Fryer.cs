using UnityEngine;

public class Fryer : Cookware
{
    protected override bool CanCook()
    {
        return true;
    }
}