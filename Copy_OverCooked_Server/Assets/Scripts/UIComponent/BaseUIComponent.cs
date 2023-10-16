using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BaseUIComponent : UIComponent
{
    private Image centerBaseImage = null;
    private List<Image> baseImages;

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

    [Obsolete]
    public override List<Image> Images 
    {
        get
        {
            return base.Images;
        }
        set
        {
            base.Images = value;
            if(value != null)
            {
                if(images.Count > 1)
                {
                    centerBaseImage.gameObject.SetActive(false);
                    SetActiveBaseImages(true);
                } 
                else
                {
                    SetActiveBaseImages(false);
                    centerBaseImage.gameObject.SetActive(true);
                }
            } 
            else
            {
                SetActiveBaseImages(false);
                centerBaseImage.gameObject.SetActive(true);
            }
        }
    }


    private void SetActiveBaseImages(bool active)
    {
        for (int i = 0; i < baseImages.Count; i++)
        {
            baseImages[i].gameObject.SetActive(active);
        }
    }

    public override void AddInstantiate(Image image)
    {
        base.AddInstantiate(image);
        if (images.Count == 1)
        {
            centerBaseImage.gameObject.SetActive(false);
            SetActiveBaseImages(true);
        }
    }

    public override void Clear()
    {
        base.Clear();
        SetActiveBaseImages(false);
        centerBaseImage.gameObject.SetActive(true);
    }

    public override void OnImagePositionUpdate()
    {
        Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(anchorTransform.position);
        Vector2 parentOffset = new Vector2(worldToScreenPos.x, worldToScreenPos.y);
        for (int i = 0; i < images.Count; i++)
        {
            Vector2 totalOffset = anchorOffset + imageOffsets[baseImages.Count - 1][i] + parentOffset;
            images[i].transform.position = totalOffset;
        }

        if(centerBaseImage.gameObject.activeSelf)
        {
            centerBaseImage.transform.position = anchorOffset + imageOffsets[0][0] + parentOffset;
        } 
        else
        {
            for(int i = 0; i < baseImages.Count; i++)
            {
                Vector2 totalOffset = anchorOffset + imageOffsets[baseImages.Count - 1][i] + parentOffset;
                baseImages[i].transform.position = totalOffset;
            }
        }

    }
}