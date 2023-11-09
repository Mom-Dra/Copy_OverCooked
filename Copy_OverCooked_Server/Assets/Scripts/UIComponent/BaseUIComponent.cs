using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BaseUIComponent : FoodUIComponent
{
    private Image centerBaseImage = null;
    private List<Image> baseImages;

    private Vector2 WorldToScreenPoint
    {
        get
        {
            return Camera.main.WorldToScreenPoint(anchorTransform.position);
        }
    }

    public BaseUIComponent(Transform anchorTransform, Vector2 offset, int maxCount) : base(anchorTransform, offset)
    {
        baseImages = new List<Image>();

        Image baseImage = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Img_PlusBase).GetComponent<Image>();

        centerBaseImage = baseImage.InstantiateOnCanvas();
        centerBaseImage.gameObject.SetActive(true);

        for (int i = 0; i < maxCount; i++)
        {
            baseImages.Add(baseImage.InstantiateOnCanvas());
        }
        OnImagePositionUpdate();
    }

    private void SetActiveAll(bool active)
    {
        centerBaseImage.gameObject.SetActive(active);
        SetActiveBaseImages(active);
    }


    private void SetActiveBaseImages(bool active)
    {
        for (int i = 0; i < baseImages.Count; i++)
        {
            baseImages[i].gameObject.SetActive(active);
        }
    }

    public override void Add(EObjectSerialCode serialCode)
    {
        if (images.Count == 0)
        {
            centerBaseImage.gameObject.SetActive(false);
            SetActiveBaseImages(true);
        } 
        else
        {
            if(images.Count > baseImages.Count)
            {
                SetActiveBaseImages(false);
            }
            else baseImages[images.Count-1].gameObject.SetActive(false);  
        }
        base.Add(serialCode);
    }

    public override void Clear(bool activeAll = false)
    {
        base.Clear();
        if (activeAll)
        {
            SetActiveAll(false);
        }   
        else
        {
            SetActiveBaseImages(false);
            centerBaseImage.gameObject.SetActive(true);
        }
        
    }

    public override void OnImagePositionUpdate()
    {
        Vector2 screenPos = WorldToScreenPoint;
        int offsetIndex = (images.Count > baseImages.Count) ? images.Count - 1 : baseImages.Count - 1;

        if (centerBaseImage.gameObject.activeSelf)
        {
            centerBaseImage.transform.position = anchorOffset + ImageOffsets[0][0] + screenPos;
        } 
        else
        {
            if(images.Count <= baseImages.Count)
            {
                for (int i = 0; i < baseImages.Count; i++)
                {
                    Vector2 totalOffset = anchorOffset + screenPos + ImageOffsets[baseImages.Count - 1][i];
                    baseImages[i].transform.position = totalOffset;
                }
            }
        }

        for (int i = 0; i < images.Count; i++)
        {
            Vector2 totalOffset = anchorOffset + screenPos + ImageOffsets[offsetIndex][i];
            images[i].transform.position = totalOffset;
        }
        
    }
}