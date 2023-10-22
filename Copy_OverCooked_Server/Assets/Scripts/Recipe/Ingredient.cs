using System;
using UnityEngine;

[Serializable]
public class Ingredient : IComparable<EObjectSerialCode>
{
    public EObjectSerialCode serial;
    public bool IsRequired = false;

    public int CompareTo(EObjectSerialCode other)
    {
        return serial - other;
    }
}