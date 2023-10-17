using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SerialCodeDictionary : MonobehaviorSingleton<SerialCodeDictionary>
{
    [Header("Serial Code Dictionary")]
    [SerializeField]
    private Dictionary<EObjectSerialCode, GameObject> serialCodDic = new Dictionary<EObjectSerialCode, GameObject>();

    private Dictionary<EObjectSerialCode, EObjectSerialCode> foodImageDictionary = new Dictionary<EObjectSerialCode, EObjectSerialCode>();

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
        Add(EObjectSerialCode.FoodBox_Tomato, "Container/FixedContainer/FoodBox/TomatoBox");
        Add(EObjectSerialCode.FoodBox_Meat, "Container/FixedContainer/FoodBox/MeatBox");
        Add(EObjectSerialCode.FoodBox_Dough, "Container/FixedContainer/FoodBox/MeatBox");

        // Cookware
        Add(EObjectSerialCode.CuttingBoard, "Container/FixedContainer/Cookware/CuttingBoard");
        Add(EObjectSerialCode.Induction, "Container/FixedContainer/Cookware/Induction");
        Add(EObjectSerialCode.Oven, "Container/FixedContainer/Cookware/Oven");

        // Tray
        Add(EObjectSerialCode.Skillet, "Container/Tray/Skillet");
        Add(EObjectSerialCode.Plate, "Container/Tray/Plate");

        // Others
        Add(EObjectSerialCode.FireExtinguisher, "Others/FireExtinguisher");

        

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

        // Food
        Add(EObjectSerialCode.Tomato, "Food/Tomato/Org_Tomato");
        Add(EObjectSerialCode.Meat, "Food/Meat/Org_Meat");
        Add(EObjectSerialCode.Chopped_Tomato, "Food/Tomato/Chopped_Tomato");
        Add(EObjectSerialCode.Chopped_Meat, "Food/Meat/Chopped_Meat");
        Add(EObjectSerialCode.Grilled_Meat, "Food/Meat/Grilled_Meat");
        Add(EObjectSerialCode.Dough, "Food/Dough/Org_Dough");
        Add(EObjectSerialCode.Chopped_Dough, "Food/Dough/Chopped_Dough");

        // Pizza
        Add(EObjectSerialCode.Tomato_Pizza, "Food/Pizza/Tomato_Pizza");


        MatchSerialCodeToFoodImage();
    }

    private void Add(EObjectSerialCode serialCode, string path)
    {
        serialCodDic.Add(serialCode, Resources.Load<GameObject>("Prefabs/"+path));
    }

    private void MatchSerialCodeToFoodImage()
    {
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Tomato, EObjectSerialCode.Img_Tomato);
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Meat, EObjectSerialCode.Img_Meat);
        foodImageDictionary.Add(EObjectSerialCode.Grilled_Meat, EObjectSerialCode.Img_Meat);
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Dough, EObjectSerialCode.Img_Dough);
    }

    public GameObject FindBySerialCode(EObjectSerialCode serialCode)
    {
        return serialCodDic[serialCode];
    }

    public EObjectSerialCode? FindFoodImageSerialCode(EObjectSerialCode serialCode)
    {
        if (foodImageDictionary.ContainsKey(serialCode))
        {
            return foodImageDictionary[serialCode];
        }
        return null;
    }

    public T InstantiateBySerialCode<T>(EObjectSerialCode serialCode) 
    {
        GameObject go = FindBySerialCode(serialCode);
        if(typeof(T) == typeof(Image))
        {
            return Instantiate(go, GameObject.Find("Canvas").transform).GetComponent<T>();
        }
        return Instantiate(go).GetComponent<T>();
    }
}