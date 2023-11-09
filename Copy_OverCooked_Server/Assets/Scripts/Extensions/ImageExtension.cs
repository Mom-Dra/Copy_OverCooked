using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension
{
    public static Image InstantiateOnCanvas(this Image image)
    {
        Image img = GameObject.Instantiate(image, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<Image>();
        return img;
    }
}