using UnityEngine;

public class Tray : Container
{
    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + displayOffset;
        }
        if (UIImage != null)
        {
            UIImage.transform.position = Camera.main.WorldToScreenPoint(transform.position + uIOffset);
        }
    }
}
