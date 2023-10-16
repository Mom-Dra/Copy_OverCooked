using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SerialCodeDictionary : MonobehaviorSingleton<SerialCodeDictionary>
{
    [Header("Serial Code Dictionary")]
    [SerializeField]
    private Dictionary<EObjectSerialCode, GameObject> serialCodDic = new Dictionary<EObjectSerialCode, GameObject>();

    protected override void Awake()
    {
        base.Awake();

        // Load Prefabs

        // Player
        Add(EObjectSerialCode.Player, "Player/player");

        // Fixed Container
        Add(EObjectSerialCode.Sink, "Container/FixedContainer/Sink");
        Add(EObjectSerialCode.Trashcan, "Container/FixedContainer/Trashcan");
        Add(EObjectSerialCode.OrderTable, "Container/FixedContainer/OrderOutTable");
        Add(EObjectSerialCode.Table, "Container/FixedContainer/Table");

        // FoodBox
        Add(EObjectSerialCode.TomatoBox, "Container/FixedContainer/FoodBox/TomatoBox");
        Add(EObjectSerialCode.MeatBox, "Container/FixedContainer/FoodBox/MeatBox");

        // Cookware
        Add(EObjectSerialCode.CuttingBoard, "Container/FixedContainer/Cookware/CuttingBoard");
        Add(EObjectSerialCode.Induction, "Container/FixedContainer/Cookware/Induction");

        // Tray
        Add(EObjectSerialCode.FryingPan, "Container/Tray/FryingPan");
        Add(EObjectSerialCode.Plate, "Container/Tray/Plate");
        
        // Others
        Add(EObjectSerialCode.FireExtinguisher, "Others/FireExtinguisher");

        // Food
        Add(EObjectSerialCode.Org_Tomato, "Food/Tomato/Org_Tomato");
        Add(EObjectSerialCode.Org_Meat, "Food/Meat/Org_Meat");
        Add(EObjectSerialCode.Chopped_Tomato, "Food/Tomato/Chopped_Tomato");
        Add(EObjectSerialCode.Grilled_Meat, "Food/Meat/Grilled_Meat");
        Add(EObjectSerialCode.Org_Dough, "Food/Dough/Org_Dough");
        Add(EObjectSerialCode.Chopped_Dough, "Food/Dough/Chopped_Dough");

        // Image
        Add(EObjectSerialCode.Img_PlusBase, "UI/Img_PlusBase");

        // State Image
        Add(EObjectSerialCode.Img_Progress, "UI/StateImage/Img_ProgressBar");
        Add(EObjectSerialCode.Img_Completed, "UI/StateImage/Img_Completed");
        Add(EObjectSerialCode.Img_Warning, "UI/StateImage/Img_Warning");
        Add(EObjectSerialCode.Img_Overheat, "UI/StateImage/Img_Overheat");

        // Food Image
        Add(EObjectSerialCode.Img_Tomato, "UI/FoodImage/Img_Tomato");
        Add(EObjectSerialCode.Img_Meat, "UI/FoodImage/Img_Meat");
        Add(EObjectSerialCode.Img_Dough, "UI/FoodImage/Img_Dough");

    }

    private void Add(EObjectSerialCode serialCode, string path)
    {
        serialCodDic.Add(serialCode, Resources.Load<GameObject>("Prefabs/" + path));
    }


    public GameObject FindBySerialCode(EObjectSerialCode serialCode)
    {
        return serialCodDic[serialCode];
    }
}