using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FoodUIComponent
{
    protected static readonly List<Vector2[]> ImageOffsets = new List<Vector2[]>(4)
    {
        { new Vector2[1]{ new Vector2(0f, 0f)} },
        { new Vector2[2]{ new Vector2(-25f, 0f), new Vector2(25f, 0f),} },
        { new Vector2[3]{ new Vector2(-25f, 50f), new Vector2(25f, 50f), new Vector2(-25f, 0f),} },
        { new Vector2[4]{ new Vector2(-25f, 50f), new Vector2(25f, 50f), new Vector2(-25f, 0f), new Vector2(25f, 0f)} }
    };
    [SerializeField]
    protected List<Image> images;
    protected Transform anchorTransform;
    protected Vector2 anchorOffset;

    public bool HasImage
    {
        get => images.Count > 0;
    }
    
    public FoodUIComponent(Transform anchorTransform, Vector2 anchorOffset)
    {
        this.anchorTransform = anchorTransform;
        this.anchorOffset = anchorOffset;
        images = new List<Image>();
    }

    public virtual void Add(EObjectSerialCode serialCode)
    {
        if (images.Count < 4)
        {
            EObjectSerialCode? foodImageSC = SerialCodeDictionary.Instance.FindFoodImageSerialCode(serialCode);
            if (foodImageSC != null)
            {
                serialCode = (EObjectSerialCode)foodImageSC;
            }
            //Debug.Log($"SC : {serialCode}, Attached : {anchorTransform.name}");
            images.Add(SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(serialCode));
            OnImagePositionUpdate();
        }
    }

    public void AddRange(List<EObjectSerialCode> serialCodeList)
    {
        foreach(EObjectSerialCode serialCode in serialCodeList)
        {
            Add(serialCode);
        }
    }

    public virtual void Clear(bool activeAll = false)
    {
        if(images != null)
        {
            foreach(Image image in images)
            {
                if(image != null)
                    GameObject.Destroy(image.gameObject);
            }
            images.Clear();
        }
    }

    public virtual void OnImagePositionUpdate()
    {
        for(int i=0; i < images.Count; i++)
        {
            Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(anchorTransform.position);
            Vector2 totalOffset = anchorOffset + ImageOffsets[images.Count - 1][i] + new Vector2(worldToScreenPos.x, worldToScreenPos.y);
            images[i].transform.position = totalOffset;
        }
    }


    
}