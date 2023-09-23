using UnityEngine;

public class FoodBox : Container
{
    [Header("Food Type")]
    [SerializeField]
    private Food food;

    public override InteractableObject Get()
    {
        InteractableObject go = Instantiate(food.gameObject, transform.position + containOffset, Quaternion.identity).GetComponent<InteractableObject>();  
        return go;
    }

    public override bool IsValidObject(InteractableObject gameObject)
    {
        return false;
    }
}
