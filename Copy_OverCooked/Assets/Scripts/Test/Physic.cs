using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        transform.position= other.transform.position;
    }
}
