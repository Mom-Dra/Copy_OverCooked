using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : InteractableObject, Containable
{
    [SerializeField]
    private float offset = 1f;

    private GameObject IObject = null;
    public GameObject Get()
    {
        return IObject;
    }

    public bool Put(GameObject gameObject)
    {
        if(IObject != null)
        {
            IObject = gameObject;
            return true;
        } 
        return false;
    }

    private void FixedUpdate()
    {
        if(IObject != null)
        {
            IObject.transform.position = transform.position + new Vector3(0, offset, 0);
        }
    }
}
