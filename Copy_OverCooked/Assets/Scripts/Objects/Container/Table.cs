using UnityEngine;

public class Table : Container
{
    public override void Fit(InteractableObject gameObject)
    {
        gameObject.transform.position = transform.position + containOffset;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public override bool IsValidObject(InteractableObject interactableObject)
    {
        return true;
    }
}
