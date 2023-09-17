using UnityEngine;

public class FoodBox : Container
{
    [SerializeField]
    private Food food;

    private void Awake()
    {
        
    }

    public override InteractableObject Get()
    {
        InteractableObject go = Instantiate(food.gameObject, transform.position + offset, Quaternion.identity).GetComponent<InteractableObject>();  
        return go;
    }
    public override void Fit(InteractableObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    public override bool IsValidObject(InteractableObject gameObject)
    {
        throw new System.NotImplementedException();
    }
}
