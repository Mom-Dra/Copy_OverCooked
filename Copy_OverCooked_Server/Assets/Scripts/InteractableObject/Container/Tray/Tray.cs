using UnityEngine;

public class Tray : Container
{
    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + displayOffset;
        }
        if (uIImage != null)
        {
            uIImage.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, uIYOffset, 0));
        }
    }
}
