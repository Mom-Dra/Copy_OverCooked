using UnityEngine;

public class FoodBox : FixedContainer
{
    [Header("Food Box")]
    [SerializeField]
    private EObjectSerialCode foodSerialCode;

    public override bool TryGet<T>(out T result, EGetMode getMode)
    {
        if (!base.TryGet(out result))
        {
            result = ObjectPullingManager.Instance.GetPullingObject(foodSerialCode).GetComponent<T>();
        }
        return result != null;
    }

    //public override EObjectType GetTopType()
    //{
    //    return EObjectType.Food;
    //}

    public override void Remove()
    {
        base.Remove();
    }

}