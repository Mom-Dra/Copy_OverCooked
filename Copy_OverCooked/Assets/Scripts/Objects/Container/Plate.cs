using UnityEngine;

public class Plate : Container
{
    public override void Fit(InteractableObject gameObject)
    {
        gameObject.transform.position = transform.position + containOffset;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }


    public override bool IsValidObject(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<Food>(out Food food))
        {
            if (food.foodState != EFoodState.Original)
            {
                return true;
            }
        }
        Debug.Log($"{this.Name} Invalid Object : Only Food has been Contained");
        return false;
    }



}
