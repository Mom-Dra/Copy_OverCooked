using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomRigidbody : MonoBehaviour
{
    private void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.mass = 55f;

        rb.isKinematic = true;
    }
}
