using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

public class ObjectPullingManager : MonobehaviorSingleton<ObjectPullingManager>
{
    [Header("Object Pulling")]
    [SerializeField]
    private int pullingNumber;

    private Dictionary<EObjectSerialCode, PullingPackage> pullingObjectList = new Dictionary<EObjectSerialCode, PullingPackage>();

    protected override void Awake()
    {
        base.Awake();
        ObjectPulling();
    }

    private void ObjectPulling()
    {
        SerializedObject[] load_Food = Resources.LoadAll<SerializedObject>("Prefabs/Food/Original");
        foreach (SerializedObject food in load_Food)
        {
            pullingObjectList.Add(food.SerialCode, new PullingPackage(pullingNumber, food.SerialCode));
        }
    }

    public GameObject GetPullingObject(EObjectSerialCode serialCode)
    {
        return pullingObjectList[serialCode].Pulling();
    }
}