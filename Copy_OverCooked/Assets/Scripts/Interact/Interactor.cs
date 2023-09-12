using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> interactableObjects;

    [HideInInspector]
    public GameObject firstInteractableObject;

    private void Awake()
    {
        interactableObjects = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if(interactableObjects.Count > 0)
        {
            GameObject prevObejct = firstInteractableObject;
            firstInteractableObject = GetClosestObject();
            Debug.Log(firstInteractableObject);
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
            interactableObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GlowOff(other.gameObject);
        interactableObjects.Remove(other.gameObject);
    }

    private void GlowOn()
    {
        if (firstInteractableObject != null)
        {
            Material material = firstInteractableObject.GetComponent<Renderer>().material;
            material.SetColor("_EmissionColor", new Color(10, 10, 10));
        }
    }

    private void GlowOff(GameObject gameObject)
    {
        if(gameObject != null)
        {
            Material material = gameObject.GetComponent<Renderer>().material;
            material.SetColor("_EmissionColor", new Color(0, 0, 0));
        }
        
    }

    public InteractableObject GetTriggeredObject()
    {
        return firstInteractableObject.GetComponent<InteractableObject>();
    }

    private GameObject GetClosestObject()
    {
        return interactableObjects.OrderBy(item => Vector3.Distance(ConvertYPositionToZero(item.transform.position), ConvertYPositionToZero(transform.position))).FirstOrDefault();        
    }

    private Vector3 ConvertYPositionToZero(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
}
