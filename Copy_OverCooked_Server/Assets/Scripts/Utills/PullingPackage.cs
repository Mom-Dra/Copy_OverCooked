using System.Linq;
using UnityEngine;

public class PullingPackage
{
    private GameObject[] pullingObjects;
    private int pullingIndex = 0;

    public PullingPackage(int pullingNumber, EObjectSerialCode serialCode)
    {
        pullingObjects = new GameObject[pullingNumber];

        Transform pullingTransform = GameObject.Find("Pulling").transform;

        GameObject pullingFolder =  new GameObject($"{serialCode.ToString()}");
        pullingFolder.transform.SetParent(pullingTransform);

        GameObject pullingObject = SerialCodeDictionary.Instance.FindBySerialCode(serialCode);
        for (int k = 0; k < pullingNumber; k++)
        {
            pullingObjects[k] = GameObject.Instantiate(pullingObject, pullingTransform.position, Quaternion.identity, pullingTransform);
            pullingObjects[k].transform.SetParent(pullingFolder.transform, false);
            pullingObjects[k].SetActive(false);
        }
    }

    public GameObject Pulling()
    {
        GameObject go = pullingObjects[pullingIndex++];
        go.SetActive(true);
        if(pullingIndex >= pullingObjects.Length)
        {
            pullingIndex = 0;
        }
        return go;
    }
}