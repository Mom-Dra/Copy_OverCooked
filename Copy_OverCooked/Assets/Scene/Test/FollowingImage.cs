using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowingImage : MonoBehaviour
{
    [SerializeField]
    private GameObject attachObject;

    [SerializeField]
    private GameObject UI;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Instantiate(UI, attachObject.transform.position, Quaternion.identity, attachObject.transform);
            Debug.Log("Attach");
        }
    }

}
