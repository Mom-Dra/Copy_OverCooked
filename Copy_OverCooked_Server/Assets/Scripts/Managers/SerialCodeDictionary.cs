using System.Collections.Generic;
using UnityEngine;

public class SerialCodeDictionary : MonobehaviorSingleton<SerialCodeDictionary>
{
    [SerializeField]
    private List<GameObject> serialCodeDictionary;

    protected override void Awake()
    {
        base.Awake();

        // Load Prefabs
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Player/player"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/Tray/FryingPan"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/Tray/Plate"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Table"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Cookware/Induction"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Cookware/CuttingBoard"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Sink"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Trashcan"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/OrderOutTable"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Others/FireExtinguisher"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/FoodBox/TomatoBox"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/FoodBox/MeatBox"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Food/Tomato/Org_Tomato"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Food/Meat/Org_Meat"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Food/Tomato/Chopped_Tomato"));
        serialCodeDictionary.Add(Resources.Load<GameObject>("Prefabs/Food/Meat/Grilled_Meat"));
    }

    public GameObject FindBySerialCode(EObjectSerialCode serialCode)
    {
        return serialCodeDictionary[(int)serialCode];
    }
}