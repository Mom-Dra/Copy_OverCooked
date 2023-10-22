using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabToCombine
{
    public EObjectSerialCode mainIngredient;
    public List<EObjectSerialCode> subIngredients;

    public GameObject mainPrefab;
    public List<GameObject> subPrefabs;

    public void SetActive(bool active)
    {
        foreach(GameObject prefab in subPrefabs)
        {
            if (prefab.gameObject.activeSelf != active)
            {
                prefab.gameObject.SetActive(active);
            }
        }
    }
}