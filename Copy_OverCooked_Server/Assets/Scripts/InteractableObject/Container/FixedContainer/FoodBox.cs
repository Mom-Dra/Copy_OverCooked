using UnityEngine;

public class FoodBox : FixedContainer
{
    [Header("Food Box")]
    [SerializeField]
    private EObjectSerialCode serialCode;

    //public override bool TryFind<T>(out T result)
    //{
    //    if (!base.TryFind<T>(out result) && typeof(T) == typeof(Food))
    //    {
    //        result = Instantiate(food, transform.position + displayOffset, Quaternion.identity).GetComponent<T>();
    //    }
    //    return result != null;
    //}

    public override bool TryGet(out InteractableObject result)
    {
        if (!base.TryGet(out result))
        {
            result = ObjectPullingManager.Instance.GetPullingObject(serialCode).GetComponent<InteractableObject>();
        }
        return result != null;
    }

    public override EObjectType GetTopType()
    {
        return EObjectType.Food;
    }
}