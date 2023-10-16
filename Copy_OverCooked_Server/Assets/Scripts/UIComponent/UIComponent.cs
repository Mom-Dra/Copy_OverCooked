using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class UIComponent
{
    protected static List<Vector2[]> imageOffsets = new List<Vector2[]>(4)
    {
        { new Vector2[1]{ new Vector2(0f, 0f)} },
        { new Vector2[2]{ new Vector2(-25f, 0f), new Vector2(25f, 0f),} },
        { new Vector2[3]{ new Vector2(-25f, 50f), new Vector2(25f, 50f), new Vector2(-25f, 0f),} },
        { new Vector2[4]{ new Vector2(-25f, 50f), new Vector2(25f, 50f), new Vector2(-25f, 0f), new Vector2(25f, 0f)} }
    };
    protected List<Image> images;
    protected Transform anchorTransform;
    protected Vector2 anchorOffset;

    public bool HasImage
    {
        get => images.Count > 0;
    }

    public Image FirstImage
    {
        get => images[0];
    }

    [Obsolete]
    public virtual List<Image> Images
    {
        get
        {
            return images;
        }
        set
        {
            if(value != null)
            {
                images = value;
                OnImagePositionUpdate();
            } 
            else
            {
                images.Clear();
            }
        }
    }       
    
    public UIComponent(Transform anchorTransform, Vector2 anchorOffset)
    {
        this.anchorTransform = anchorTransform;
        this.anchorOffset = anchorOffset;
        images = new List<Image>();
    }

    public virtual void Add(EObjectSerialCode serialCode)
    {
        if (images.Count < 4)
        {
            Image image = SerialCodeDictionary.Instance.FindBySerialCode(serialCode).GetComponent<Image>();
            AddInstantiate(image);
        }
    }

    public virtual void AddInstantiate(Image image)
    {
        if (images.Count < 4)
        {
            images.Add(image.InstantiateOnCanvas());
            OnImagePositionUpdate();
        }
    }

    public virtual void Clear()
    {
        foreach(Image image in images)
        {
            GameObject.Destroy(image.gameObject);
        }
        images.Clear();
    }

    public virtual void OnImagePositionUpdate()
    {
        for(int i=0; i < images.Count; i++)
        {
            Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(anchorTransform.position);
            Vector2 totalOffset = anchorOffset + imageOffsets[images.Count - 1][i] + new Vector2(worldToScreenPos.x, worldToScreenPos.y);
            images[i].transform.position = totalOffset;
        }
    }


    
}