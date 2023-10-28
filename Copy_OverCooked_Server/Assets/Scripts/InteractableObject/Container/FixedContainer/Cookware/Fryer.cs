using UnityEngine;

public class Fryer : Cookware
{
    [Header("Fryer")]
    [SerializeField]
    private Vector3 onCookTrayPos = new Vector3 (0, 0.35f, 0);

    public override bool TryPut(InteractableObject interactableObject)
    {
        if (base.TryPut(interactableObject))
        {
            if (TryGet<Food>(out Food food))
            {
                TryCook();
            }
            return true;
        }
        return false;
    }
    protected override bool CanCook()
    {
        return TryGet<FryerTray>(out FryerTray tray);
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return !HasObject() && interactableObject.TryGetComponent<FryerTray>(out FryerTray fryerTray);
    }

    protected override void ThrowPut(InteractableObject interactableObject)
    {
        if (!TryCook())
        {
            base.ThrowPut(interactableObject);
        }
    }

    public override void OnProgressBegin()
    {
        if(getObject.TryGetComponent<FryerTray>(out FryerTray fryerTray))
        {
            fryerTray.transform.position = transform.position + onCookTrayPos;
        }
    }

    public override void OnProgressEnd()
    {
        //if (getObject.TryGetComponent<FryerTray>(out FryerTray fryerTray))
        //{
        //    fryerTray.transform.position = transform.position + displayOffset;
        //}
    }
}