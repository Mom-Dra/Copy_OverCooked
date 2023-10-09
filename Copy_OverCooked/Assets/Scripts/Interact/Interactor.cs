using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField]
    private float brightness = 0.4f;

    [Header("Debug")]
    [SerializeField]
    private List<InteractableObject> interactableObjects;

    public InteractableObject ClosestInteractableObject;

    private void Awake()
    {
        interactableObjects = new List<InteractableObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null && interactableObject.IsInteractable)
        {
            interactableObjects.Add(interactableObject);
            SetClosestObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject io = other.GetComponent<InteractableObject>();
        interactableObjects.Remove(io);
        SetClosestObject();
    }

    public void SetClosestObject()
    {
        ClosestInteractableObject = interactableObjects.OrderBy(item => Vector3.Distance(ConvertYPositionToZero(item.transform.position), ConvertYPositionToZero(transform.position + Vector3.forward)))
        .FirstOrDefault();
    }

    private Vector3 ConvertYPositionToZero(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
}