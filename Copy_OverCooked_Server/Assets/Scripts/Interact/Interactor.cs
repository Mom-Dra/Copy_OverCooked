using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactor : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    private List<InteractableObject> triggerObjectList = new List<InteractableObject>();
    [SerializeField]
    private InteractableObject triggerObject;

    // Property
    public InteractableObject TriggerObject 
    { 
        get => triggerObject; 
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null && interactableObject.Selectable)
        {
            interactableObject.inclusiveInteractor = this;
            triggerObjectList.Add(interactableObject);
            SetClosestObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InteractableObject>(out InteractableObject io))
        {
            io.inclusiveInteractor = null;
            triggerObjectList.Remove(io);
            SetClosestObject();
        }
    }

    private void SetClosestObject()
    {
        InteractableObject prevTriggered = triggerObject;
        if(triggerObjectList.Count > 0)
        {
            triggerObject = triggerObjectList.OrderBy(item => Vector3.Distance(ConvertYPositionToZero(item.transform.position), ConvertYPositionToZero(transform.position + Vector3.forward)))
            .FirstOrDefault();
        } else
        {
            triggerObject = null;
        }

        if(prevTriggered != triggerObject)
        {
            prevTriggered?.GlowOff();
            triggerObject?.GlowOn();
        }
    }

    private Vector3 ConvertYPositionToZero(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    public void RemoveOnDestroy(InteractableObject interactableObject)
    {
        interactableObject.inclusiveInteractor = null;
        triggerObjectList.Remove(interactableObject);
        SetClosestObject();
    }
}