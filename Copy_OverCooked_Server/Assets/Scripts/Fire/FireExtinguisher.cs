using UnityEngine;

public class FireExtinguisher : InteractableObject, IReactable
{
    private GameObject waterPrefab;

    private bool working = false;

    protected override void Start()
    {
        base.Start();
        GameObject _prefab = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Water);
        waterPrefab = Instantiate(_prefab, transform.position, transform.rotation, transform);
    }

    public void React(Player player)
    {
        if(working)
        {
            waterPrefab.SetActive(false);
            working = false;
        } 
        else
        {
            waterPrefab.SetActive(true);
            working = true;
        }
        
    }
}