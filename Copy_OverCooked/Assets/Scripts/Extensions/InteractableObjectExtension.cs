using UnityEngine;

public static class InteractableObjectExtension
{
    public static void Fix(this InteractableObject interactableObject)
    {
        Rigidbody rb = interactableObject.GetComponent<Rigidbody>();
        if(!rb.isKinematic)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.Euler(0f, 0f, 0f);
            rb.isKinematic = true;
        }
    }

    public static void Free(this InteractableObject interactableObject)
    {
        Rigidbody rb = interactableObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public static void RemoveFromInteractor(this InteractableObject interactableObject)
    {
        foreach (Interactor interactor in Interactor.interactors)
            interactor.RemoveObject(interactableObject);

        GameObject.Destroy(interactableObject.gameObject);
    }

    public static Player GetLinkedPlayer(this InteractableObject interactableObject)
    {
        return LinkManager.Instance.GetLinkedPlayer(interactableObject);
    }
}