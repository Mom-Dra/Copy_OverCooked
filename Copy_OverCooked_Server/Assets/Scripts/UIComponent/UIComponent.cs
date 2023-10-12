using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIComponent
{
    private static List<Vector2[]> m_imageOffsets = new List<Vector2[]>(4)
    {
        { new Vector2[1]{ new Vector2(50f, 0f)} },
        { new Vector2[2]{ new Vector2(1f, 1f), new Vector2(1f, 1f),} },
        { new Vector2[3]{ new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),} },
        { new Vector2[4]{ new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f)} }
    };
    private Image[] m_images = new Image[4];
    private int m_index = 0;
    private Transform m_anchorTransform;
    private Vector3 m_anchorOffset;

    public int Count
    {
        get => m_index;
    }

    public Image Images
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

    public UIComponent(Transform anchorTransform, Vector3 anchorOffset)
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
        for(int i=0; i < m_index - 1; i++)
        {
            Vector2 offset = m_imageOffsets[m_index][i];
            Debug.Log(offset);
            m_images[i].transform.position = m_anchorOffset + Camera.main.WorldToScreenPoint(m_anchorTransform.position) + new Vector3(offset.x, 0f, offset.y);
        }
    }


    
}