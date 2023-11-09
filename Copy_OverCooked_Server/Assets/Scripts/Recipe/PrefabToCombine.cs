using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabToCombine
{
    public List<EObjectSerialCode> ingredients;
    public List<GameObject> prefabs;

    public void SetActive(bool active)
    {
        foreach(GameObject prefab in prefabs)
        {
            if (prefab.gameObject.activeSelf != active)
            {
                prefab.gameObject.SetActive(active);
            }
        }
    }
}