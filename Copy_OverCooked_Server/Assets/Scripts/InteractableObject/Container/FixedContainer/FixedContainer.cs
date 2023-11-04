using UnityEngine;

public class FixedContainer : Container
{
    protected FireTriggerBox fireTriggerBox;

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
