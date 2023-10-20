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

        for (int k = 0; k < pullingNumber; k++)
        {
            GameObject pullingObject = SerialCodeDictionary.Instance.FindBySerialCode(serialCode);
            pullingObjects[k] = GameObject.Instantiate(pullingObject, pullingTransform.position, Quaternion.identity, pullingTransform);
            pullingObjects[k].SetActive(false);
            pullingObjects[k].transform.SetParent(pullingFolder.transform, false);
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