using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{
    private List<GameObject> grabbables;
    private List<GameObject> interactables;

    private void Awake()
    {
        grabbables = new List<GameObject>();
        interactables = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            grabbables.Add(other.gameObject);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            interactables.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
        {
            grabbables.Remove(other.gameObject);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            grabbables.Remove(other.gameObject);
        }
    }

    public GameObject GetClosestGrabbable()
    {
        return grabbables.OrderBy(item => Vector3.Distance(item.transform.position, transform.position)).FirstOrDefault();        
    }

    public GameObject GetClosestInteractable()
    {
        return interactables.OrderBy(item => Vector3.Distance(item.transform.position, transform.position)).FirstOrDefault();
    }
}
