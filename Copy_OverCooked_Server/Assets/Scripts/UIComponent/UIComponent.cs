using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComponent
{
    private static List<Vector2[]> m_imageOffsets = new List<Vector2[]>(4)
    {
        { new Vector2[1]{ new Vector2(0f, 0f)} },
        { new Vector2[2]{ new Vector2(-25f, 0f), new Vector2(25f, 0f),} },
        { new Vector2[3]{ new Vector2(-25f, 50f), new Vector2(25f, 50f), new Vector2(-25f, 0f),} },
        { new Vector2[4]{ new Vector2(-25f, 50f), new Vector2(25f, 50f), new Vector2(-25f, 0f), new Vector2(25f, 0f)} }
    };
    private Image[] m_images = new Image[4];
    private int m_index = 0;
    private Transform m_anchorTransform;
    private Vector2 m_anchorOffset;

    public int Index
    {
        get => m_index;
    }

    public Image Image
    {
        get
        {
            if(m_index == 0)
            {
                return m_images[0];
            }
            return m_images[m_index - 1];
        }
        set
        {
            if(value == null && m_index > 0)
            {
                DestroyAllImages();
            }
            else if(m_index < 4)
            {
                m_images[m_index++] = value;
                OnUIPositionChanging();
            }
        }
    }

    public Image[] Images
    {
        get 
        { 
            return m_images; 
        }
        set 
        {
            m_images = value; 
            // m_index Ã³¸® 
        }
    }

    public UIComponent(Transform anchorTransform, Vector2 anchorOffset)
    {
        m_anchorTransform = anchorTransform;
        m_anchorOffset = anchorOffset;
    }

    private void DestroyAllImages()
    {
        for(int i=0; i<m_index; ++i)
        {
            GameObject.Destroy(m_images[i].gameObject);
            m_images[i] = null;
        }
        m_index = 0;
    }

    public void OnUIPositionChanging()
    {
        for(int i=0; i < m_index; i++)
        {
            Vector3 worldToScreenPos = Camera.main.WorldToScreenPoint(m_anchorTransform.position);
            Vector2 totalOffset = m_anchorOffset + m_imageOffsets[m_index - 1][i] + new Vector2(worldToScreenPos.x, worldToScreenPos.y);
            m_images[i].transform.position = totalOffset;
        }
    }


    
}