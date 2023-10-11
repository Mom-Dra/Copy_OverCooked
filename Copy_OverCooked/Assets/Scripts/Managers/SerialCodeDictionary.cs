using System.Collections.Generic;
using UnityEngine;

public class SerialCodeDictionary : MonobehaviorSingleton<SerialCodeDictionary>
{
    [SerializeField]
    private List<GameObject> serialCodeDictionary;

    public GameObject FindBySerialCode(EObjectSerialCode serialCode)
    {
        return serialCodeDictionary[(int)serialCode];
    }
}