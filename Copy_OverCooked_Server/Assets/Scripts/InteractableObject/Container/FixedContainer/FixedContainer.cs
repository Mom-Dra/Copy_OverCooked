using UnityEngine;

public abstract class FixedContainer : Container
{
    [Header("Fixed Container")]
    [SerializeField]
    protected bool flammablity = true; // °¡¿¬¼º 

    protected FireTriggerBox fireTriggerBox;

    // Property
    public bool Flammablity
    {
        get => flammablity;
    }

    protected override void Start()
    {
        base.Start();
        GameObject _prefab = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.FireTriggerBox);
        fireTriggerBox = Instantiate(_prefab, transform.position + Vector3.up * 2f, Quaternion.identity, transform).GetComponent<FireTriggerBox>();
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return base.IsValidObject(interactableObject) && !fireTriggerBox.OnFire;
    }

    protected override void ThrowPut(InteractableObject interactableObject)
    {
        if (!HasObject() && !fireTriggerBox.OnFire)
        {
            TryPut(interactableObject);
        }
    }
}
