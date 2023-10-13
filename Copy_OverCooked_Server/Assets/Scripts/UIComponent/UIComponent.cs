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
    private int offsetIndex = -1;

    private int OffsetIndex
    {
        get
        {
            return offsetIndex == -1 ? index - 1 : offsetIndex - 1;
        }
    }

    public int Count
    {
        get => index;
    }

    public Image[] Images
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

    public void Add(Image image)
    {
        if (index < 4)
        {
            images[index++] = image;
        }
    }

    public void SetOffsetIndex(int index)
    {
        offsetIndex = index;
    }

    public void DestroyAllImages()
    {
        for(int i=0; i<index; ++i)
        {
            GameObject.Destroy(images[i].gameObject);
            images[i] = null;
        }
        index = 0;
    }

    public void OnUIPositionChanging(Transform parent, Vector2 anchorOffset)
    {
        for(int i=0; i < index; i++)
        {
            Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(parent.position);
            Vector2 totalOffset = anchorOffset + imageOffsets[OffsetIndex][i] + new Vector2(worldToScreenPos.x, worldToScreenPos.y);
            images[i].transform.position = totalOffset;
        }
    }


    
}