using System.Collections.Generic;
using UnityEngine;

public class SerialCodeDictionary : MonobehaviorSingleton<SerialCodeDictionary>
{
    [SerializeField]
    private List<GameObject> serialCodeList;

    protected override void Awake()
    {
        base.Awake();

        // Load Prefabs
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Player/player"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/Tray/FryingPan"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/Tray/Plate"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Table"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Cookware/Induction"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Cookware/CuttingBoard"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Sink"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/Trashcan"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/OrderOutTable"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Others/FireExtinguisher"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/FoodBox/TomatoBox"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Container/FixedContainer/FoodBox/MeatBox"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Food/Tomato/Org_Tomato"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Food/Meat/Org_Meat"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Food/Tomato/Chopped_Tomato"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/Food/Meat/Grilled_Meat"));
        serialCodeList.Add(Resources.Load<GameObject>("Prefabs/UI/FoodImage/Img_Tomato"));
    }

    public GameObject FindBySerialCode(EObjectSerialCode serialCode)
    {
        return serialCodeList[(int)serialCode];
    }
}