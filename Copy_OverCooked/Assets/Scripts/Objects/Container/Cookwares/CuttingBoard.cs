using UnityEngine;

public class CuttingBoard : Cookware
{
    public override void Fit(InteractableObject gameObject)
    {
        gameObject.transform.position = transform.position + offset;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.isKinematic = true;
    }

    public override bool IsValidObject(InteractableObject gameObject)
    {
        return true;
    }
}
