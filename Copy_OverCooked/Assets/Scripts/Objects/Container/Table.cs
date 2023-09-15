using UnityEngine;

public class Table : Container
{
    public override void Fit(InteractableObject gameObject)
    {
        gameObject.transform.position = transform.position + offset;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public override bool IsValidObject(InteractableObject gameObject)
    {
        return true;
    }
}
