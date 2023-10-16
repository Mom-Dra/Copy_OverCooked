using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension
{
    public static Image InstantiateOnCanvas(this Image image)
    {
        return GameObject.Instantiate(image, GameObject.Find("Canvas").transform).GetComponent<Image>();
    }
}