using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tray : Container
{
    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + containOffset;
        }
    }
}
