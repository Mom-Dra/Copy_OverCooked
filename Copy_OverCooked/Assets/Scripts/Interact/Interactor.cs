using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private List<InteractableObject> interactableObjects;

    [HideInInspector]
    public InteractableObject firstInteractableObject;

    private void Awake()
    {
        interactableObjects = new List<InteractableObject>();
    }

    private void FixedUpdate() // Update 할 필요가 있니? --> 추가 되거나, 빠질때만 하면된다
    {
        if(interactableObjects.Count > 0)
        {
            InteractableObject prevObejct = firstInteractableObject;
            firstInteractableObject = GetClosestObject();
            //Debug.Log(firstInteractableObject);
            if(prevObejct != firstInteractableObject)
            {
                GlowOn();
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null && interactableObject.IsInteractable)
        {
            interactableObjects.Add(interactableObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject io = other.GetComponent<InteractableObject>();
        GlowOff(io);
        interactableObjects.Remove(io);
    }

    private void GlowOn()
    {
        if (firstInteractableObject != null)
        {
            Material material = firstInteractableObject.GetComponent<MeshRenderer>().material;
            material.SetColor("_EmissionColor", new Color(10, 10, 10));
        }
    }

    private void GlowOff(InteractableObject gameObject)
    {
        if(gameObject != null)
        {
            Material material = gameObject.GetComponent<MeshRenderer>().material;
            material.SetColor("_EmissionColor", new Color(0, 0, 0));
        }
    }

    public InteractableObject GetTriggeredObject()
    {
        InteractableObject go = firstInteractableObject;
        interactableObjects.Remove(go);
        return go;
    }

    private InteractableObject GetClosestObject()
    {
        return interactableObjects.OrderBy(item => Vector3.Distance(ConvertYPositionToZero(item.transform.position), ConvertYPositionToZero(transform.position)))
            .FirstOrDefault().GetComponent<InteractableObject>();      
    }

    private Vector3 ConvertYPositionToZero(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
}
