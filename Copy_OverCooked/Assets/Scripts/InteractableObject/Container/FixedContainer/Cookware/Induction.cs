using UnityEngine;

public class Induction : Cookware
{
    protected override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
        if(TryGet<Food>(out Food food))
        {
            TryCook();
        }
    }
    protected override bool CanCook()
    {
        return true;
    }
}