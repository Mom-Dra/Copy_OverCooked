using Unity.VisualScripting;
using UnityEngine;

public static class InteractableObjectExtension
{
    public static void FixFromExternalPhysics(this InteractableObject interactableObject)
    {
        Rigidbody rb = interactableObject.GetComponent<Rigidbody>();
        rb.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.isKinematic = true;
    }

    public static void Free(this InteractableObject interactableObject)
    {
        Rigidbody rb = interactableObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }
}