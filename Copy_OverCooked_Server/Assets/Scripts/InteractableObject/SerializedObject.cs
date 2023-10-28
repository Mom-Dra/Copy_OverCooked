using System;
using System.Xml.Linq;
using UnityEngine;

public class SerializedObject : MonoBehaviour
{
    private static int s_id;

    [SerializeField]
    protected EObjectSerialCode serialCode;
    [SerializeField]
    protected int id; // For Debug

    // Property
    public int Id { get => id; }

    public EObjectSerialCode SerialCode
    {
        get => serialCode;
    }

    protected virtual void Awake()
    {
        id = s_id++;
    }

    protected virtual void Start()
    {
        NetworkObjectManager.Instance.Add(this);
    }

    public override bool Equals(object other)
    {
        SerializedObject so = other as SerializedObject;
        return id == so.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), id);
    }

}