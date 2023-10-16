using System.Collections.Generic;
using UnityEngine;

public class Induction : Cookware
{
    public override bool TryPut(SendObjectArgs sendContainerArgs)
    {
        if (base.TryPut(sendContainerArgs))
        {
            if (TryFind<Food>(out Food food))
            {
                TryCook();
            }
            return true;
        }
        return false;
    }

    protected override bool IsValidObject(List<EObjectSerialCode> serialObjects)
    {
        return base.IsValidObject(serialObjects);
    }

    protected override bool CanCook()
    {
        return true;
    }
}