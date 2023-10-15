using System.Collections.Generic;
using UnityEngine;

public class ObjectPullingManager : MonobehaviorSingleton<ObjectPullingManager>
{
    [Header("Object Pulling")]
    [SerializeField]
    private int pullingNumber;
    [SerializeField]
    private List<EObjectSerialCode> serialCodeForPulling;

    private List<PullingPackage> pullingObjectList = new List<PullingPackage>();

    protected override void Awake()
    {
        base.Awake();
        ObjectPulling();
    }

    private void ObjectPulling()
    {
        for (int i = 0; i < serialCodeForPulling.Count; ++i)
        {
            pullingObjectList.Add(new PullingPackage(pullingNumber, serialCodeForPulling[i]));
        }
    }

    public GameObject GetPullingObject(EObjectSerialCode serialCode)
    {
        int listIndex = serialCodeForPulling.IndexOf(serialCode);
        return pullingObjectList[listIndex].Pulling();
    }
}