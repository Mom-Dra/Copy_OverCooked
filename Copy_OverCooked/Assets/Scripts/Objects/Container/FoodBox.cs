using UnityEngine;

public class FoodBox : Container
{
    [Header("Food Type")]
    [SerializeField]
    private Food food;

    protected override void Awake()
    {
        
    }

    public override InteractableObject Get()
    {
        InteractableObject go = Instantiate(food.gameObject, transform.position + containOffset, Quaternion.identity).GetComponent<InteractableObject>();  
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
