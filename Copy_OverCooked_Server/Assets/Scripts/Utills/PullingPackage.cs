using UnityEngine;

public class PullingPackage
{
    private GameObject[] pullingObjects;
    private int pullingIndex = 0;

    public PullingPackage(int pullingNumber, EObjectSerialCode serialCode)
    {
        pullingObjects = new GameObject[pullingNumber];

        for (int k = 0; k < pullingNumber; k++)
        {
            GameObject pullingObject = SerialCodeDictionary.Instance.FindBySerialCode(serialCode);
            pullingObjects[k] = GameObject.Instantiate(pullingObject, GameObject.Find("Managers").transform.position, Quaternion.identity);
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