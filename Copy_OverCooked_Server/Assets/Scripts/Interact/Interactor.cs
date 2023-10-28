using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactor : MonoBehaviour
{
    [Header("Trigger")]
    //[SerializeField]
    //private float brightness = 0.4f;

    [Header("Debug")]
    [SerializeField]
    private List<InteractableObject> triggerObjectList = new List<InteractableObject>();

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
            triggerObjectList.Add(interactableObject);
            SetClosestObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject io = other.GetComponent<InteractableObject>();
        triggerObjectList.Remove(io);
        SetClosestObject();
    }

    public void SetClosestObject()
    {
        InteractableObject prevTriggered = TriggerObject;
        triggerObject = triggerObjectList.OrderBy(item => Vector3.Distance(ConvertYPositionToZero(item.transform.position), ConvertYPositionToZero(transform.position + Vector3.forward)))
        .FirstOrDefault();

        if(prevTriggered != triggerObject)
        {
            //prevTriggered?.GlowOff();
            //triggerObject?.GlowOn();
        }
    }

    private Vector3 ConvertYPositionToZero(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
}