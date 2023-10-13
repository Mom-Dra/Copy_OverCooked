using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComponent
{
    private static List<Vector2[]> imageOffsets = new List<Vector2[]>(4)
    {
        { new Vector2[1]{ new Vector2(0f, 0f)} },
        { new Vector2[2]{ new Vector2(-25f, 0f), new Vector2(25f, 0f),} },
        { new Vector2[3]{ new Vector2(-25f, 50f), new Vector2(25f, 50f), new Vector2(-25f, 0f),} },
        { new Vector2[4]{ new Vector2(-25f, 50f), new Vector2(25f, 50f), new Vector2(-25f, 0f), new Vector2(25f, 0f)} }
    };
    private Image[] images = new Image[4];
    private int index = 0;
    private Transform anchorTransform;
    private Vector2 anchorOffset;

    public int Index
    {
        get => index;
    }

    public Image[] Image
    {
        get
        {
            return images;
        }
        set
        {
            images = value;
        }
    }        

    //if(value == null && m_index > 0)
    //{
    //    DestroyAllImages();
    //}
    //else if(m_index < 4)
    //{
    //    m_images[m_index++] = value;
    //    OnUIPositionChanging();
    //}

    public Image[] Images
    {
        get 
        { 
            return images; 
        }
        set 
        {
            images = value; 
            // m_index Ã³¸® 
        }
    }

    public UIComponent(Transform anchorTransform, Vector2 anchorOffset)
    {
        this.anchorTransform = anchorTransform;
        this.anchorOffset = anchorOffset;
    }

    private void DestroyAllImages()
    {
        for(int i=0; i<index; ++i)
        {
            GameObject.Destroy(images[i].gameObject);
            images[i] = null;
        }
        index = 0;
    }

    public void OnUIPositionChanging()
    {
        for(int i=0; i < index; i++)
        {
            Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(anchorTransform.position);
            Vector2 totalOffset = anchorOffset + imageOffsets[index - 1][i] + new Vector2(worldToScreenPos.x, worldToScreenPos.y);
            images[i].transform.position = totalOffset;
        }
    }


    
}