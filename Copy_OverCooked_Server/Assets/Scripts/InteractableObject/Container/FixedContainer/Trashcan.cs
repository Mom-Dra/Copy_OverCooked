using UnityEngine;

public class Trashcan : FixedContainer
{

    public override bool TryPut(InteractableObject interactableObject)
    {
        Debug.Log("Trashcan TryPut");
        Destroy(interactableObject.gameObject);
        Debug.Log("Success");
        return true;
    }
}