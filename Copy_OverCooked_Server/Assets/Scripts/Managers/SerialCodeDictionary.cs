using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
        Add(EObjectSerialCode.FoodBox_Cheeze, "Container/FixedContainer/FoodBox/CheezeBox");

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
        Add(EObjectSerialCode.Img_Cheeze, "UI/FoodImage/Img_Cheeze");
        Add(EObjectSerialCode.Img_Cabbage, "UI/FoodImage/Img_Cabbage");
        Add(EObjectSerialCode.Img_Carrot, "UI/FoodImage/Img_Carrot");
        Add(EObjectSerialCode.Img_Bread, "UI/FoodImage/Img_Bread");
        Add(EObjectSerialCode.Img_Chicken, "UI/FoodImage/Img_Chicken");
        Add(EObjectSerialCode.Img_Egg, "UI/FoodImage/Img_Egg");
        Add(EObjectSerialCode.Img_Honey, "UI/FoodImage/Img_Honey");
        Add(EObjectSerialCode.Img_Chocolate, "UI/FoodImage/Img_Chocolate");
        Add(EObjectSerialCode.Img_Fish, "UI/FoodImage/Img_Fish");
        Add(EObjectSerialCode.Img_Sausage, "UI/FoodImage/Img_Sausage");
        Add(EObjectSerialCode.Img_Seaweed, "UI/FoodImage/Img_Seaweed");
        Add(EObjectSerialCode.Img_Shrimp, "UI/FoodImage/Img_Shrimp");
        Add(EObjectSerialCode.Img_Flour, "UI/FoodImage/Img_Flour");
        Add(EObjectSerialCode.Img_Cucumber, "UI/FoodImage/Img_Cucumber");
        Add(EObjectSerialCode.Img_Mushroom, "UI/FoodImage/Img_Mushroom");
        Add(EObjectSerialCode.Img_Noodle, "UI/FoodImage/Img_Noodle");
        Add(EObjectSerialCode.Img_Rice, "UI/FoodImage/Img_Rice");
        Add(EObjectSerialCode.Img_Pineapple, "UI/FoodImage/Img_Pineapple");
        Add(EObjectSerialCode.Img_Tortilla, "UI/FoodImage/Img_Tortilla");

        // Pizza
        Add(EObjectSerialCode.Tomato_Pizza, "Food/Pizza/Tomato_Pizza");

        // Original Food
        Add(EObjectSerialCode.Tomato, "Food/Tomato/Tomato");
        Add(EObjectSerialCode.Meat, "Food/Meat/Meat");
        Add(EObjectSerialCode.Dough, "Food/Dough/Dough");
        Add(EObjectSerialCode.Cheeze, "Food/Cheeze/Cheeze");
        Add(EObjectSerialCode.Cabbage, "Food/Cabbage/Cabbage");
        Add(EObjectSerialCode.Carrot, "Food/Carrot/Carrot");
        Add(EObjectSerialCode.Cucumber, "Food/Cucumber/Cucumber");
        Add(EObjectSerialCode.Bread, "Food/Bread/Bread");
        Add(EObjectSerialCode.Chicken, "Food/Chicken/Chicken");
        Add(EObjectSerialCode.Noodle, "Food/Noodle/Noodle");
        Add(EObjectSerialCode.Sausage, "Food/Sausage/Sausage");
        Add(EObjectSerialCode.Seaweed, "Food/Seaweed/Seaweed");
        Add(EObjectSerialCode.Shrimp, "Food/Shrimp/Shrimp");
        Add(EObjectSerialCode.Fish, "Food/Fish/Fish");
        Add(EObjectSerialCode.Rice, "Food/Rice/Rice");
        Add(EObjectSerialCode.Chocolate, "Food/Chocolate/Chocolate");
        Add(EObjectSerialCode.Honey, "Food/Honey/Honey");
        Add(EObjectSerialCode.Tortilla, "Food/Tortilla/Tortilla");
        Add(EObjectSerialCode.Mushroom, "Food/Mushroom/Mushroom");
        Add(EObjectSerialCode.Pineapple, "Food/Pineapple/Pineapple");
        Add(EObjectSerialCode.Flour, "Food/Flour/Flour");
        Add(EObjectSerialCode.Egg, "Food/Egg/Egg");
        Add(EObjectSerialCode.Potato, "Food/Potato/Potato");

        // Chopped
        Add(EObjectSerialCode.Chopped_Tomato, "Food/Tomato/Chopped_Tomato");
        Add(EObjectSerialCode.Chopped_Meat, "Food/Meat/Chopped_Meat");
        Add(EObjectSerialCode.Chopped_Dough, "Food/Dough/Chopped_Dough");
        Add(EObjectSerialCode.Chopped_Cheeze, "Food/Cheeze/Chopped_Cheeze");
        Add(EObjectSerialCode.Chopped_Carrot, "Food/Carrot/Chopped_Carrot");
        Add(EObjectSerialCode.Chopped_Cucumber, "Food/Cucumber/Chopped_Cucumber");
        Add(EObjectSerialCode.Chopped_Cabbage, "Food/Cabbage/Chopped_Cabbage");
        Add(EObjectSerialCode.Chopped_Chicken, "Food/Chicken/Chopped_Chicken");
        Add(EObjectSerialCode.Chopped_Chocolate, "Food/Chocolate/Chopped_Chocolate");
        Add(EObjectSerialCode.Chopped_Honey, "Food/Honey/Chopped_Honey");
        Add(EObjectSerialCode.Chopped_Shrimp, "Food/Shrimp/Chopped_Shrimp");
        Add(EObjectSerialCode.Chopped_Mushroom, "Food/Mushroom/Chopped_Mushroom");
        Add(EObjectSerialCode.Chopped_Pineapple, "Food/Pineapple/Chopped_Pineapple");
        Add(EObjectSerialCode.Chopped_Fish, "Food/Fish/Chopped_Fish");

        // Grilled
        Add(EObjectSerialCode.Grilled_Tomato, "Food/Tomato/Grilled_Tomato");
        Add(EObjectSerialCode.Grilled_Meat, "Food/Meat/Grilled_Meat");
        Add(EObjectSerialCode.Grilled_Chicken, "Food/Chicken/Grilled_Chicken");
        Add(EObjectSerialCode.Grilled_Fish, "Food/Fish/Grilled_Fish");
        Add(EObjectSerialCode.Grilled_Mushroom, "Food/Mushroom/Grilled_Mushroom");
        Add(EObjectSerialCode.Grilled_Shrimp, "Food/Shrimp/Grilled_Shrimp");

        // Baked
        Add(EObjectSerialCode.Baked_Dough, "Food/Dough/Baked_Dough");

        // Topping On Pizza
        Add(EObjectSerialCode.Tomato_Topping, "Food/Pizza/Topping/Tomato_Topping");
        Add(EObjectSerialCode.Cheeze_Topping, "Food/Pizza/Topping/Cheeze_Topping");

        // Boiled
        Add(EObjectSerialCode.Boiled_Noodle, "Food/Pizza/Noddle/Boiled_Noodle");
        Add(EObjectSerialCode.Boiled_Rice, "Food/Pizza/Rice/Boiled_Rice");

        // Fried
        Add(EObjectSerialCode.Fried_Chicken, "Food/Pizza/Chicken/Fried_Chicken");
        Add(EObjectSerialCode.Fried_Potato, "Food/Pizza/Potato/Fried_Potato");



        


        MatchSerialCodeToFoodImage();
    }

    private void Add(EObjectSerialCode serialCode, string path)
    {
        serialCodDic.Add(serialCode, Resources.Load<GameObject>("Prefabs/"+path));
    }

    private void MatchSerialCodeToFoodImage()
    {
        //Tomato,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Tomato, EObjectSerialCode.Img_Tomato);
        foodImageDictionary.Add(EObjectSerialCode.Grilled_Tomato, EObjectSerialCode.Img_Tomato);
        foodImageDictionary.Add(EObjectSerialCode.Tomato_Topping, EObjectSerialCode.Img_Tomato);
        //Meat,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Meat, EObjectSerialCode.Img_Meat);
        foodImageDictionary.Add(EObjectSerialCode.Grilled_Meat, EObjectSerialCode.Img_Meat);
        //Chicken,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Chicken, EObjectSerialCode.Img_Chicken);
        foodImageDictionary.Add(EObjectSerialCode.Fried_Chicken, EObjectSerialCode.Img_Chicken);
        foodImageDictionary.Add(EObjectSerialCode.Grilled_Chicken, EObjectSerialCode.Img_Chicken);
        //Dough,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Dough, EObjectSerialCode.Img_Dough);
        foodImageDictionary.Add(EObjectSerialCode.Baked_Dough, EObjectSerialCode.Img_Dough);
        //Cheeze,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Cheeze, EObjectSerialCode.Img_Cheeze);
        foodImageDictionary.Add(EObjectSerialCode.Cheeze_Topping, EObjectSerialCode.Img_Cheeze);
        //Cabbage,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Cabbage, EObjectSerialCode.Img_Cabbage);
        //Pineapple,
        foodImageDictionary.Add(EObjectSerialCode.Pineapple, EObjectSerialCode.Img_Pineapple);
        foodImageDictionary.Add(EObjectSerialCode.Pineapple_Topping, EObjectSerialCode.Img_Pineapple);
        //Cucumber, 
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Cucumber, EObjectSerialCode.Img_Cucumber);
        //Onion,
        //Corn,
        //Shrimp,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Shrimp, EObjectSerialCode.Img_Shrimp);
        foodImageDictionary.Add(EObjectSerialCode.Grilled_Shrimp, EObjectSerialCode.Img_Shrimp);

        //Fish,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Fish, EObjectSerialCode.Img_Fish);
        foodImageDictionary.Add(EObjectSerialCode.Grilled_Fish, EObjectSerialCode.Img_Fish);
        //Carrot,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Carrot, EObjectSerialCode.Img_Carrot);
        foodImageDictionary.Add(EObjectSerialCode.Plating_Carrot, EObjectSerialCode.Img_Carrot);
        //Mushroom,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Mushroom, EObjectSerialCode.Img_Mushroom);
        foodImageDictionary.Add(EObjectSerialCode.Grilled_Mushroom, EObjectSerialCode.Img_Mushroom);
        //Potato,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Potato, EObjectSerialCode.Img_Potato);
        foodImageDictionary.Add(EObjectSerialCode.Fried_Potato, EObjectSerialCode.Img_Potato);
        //Sausage,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Sausage, EObjectSerialCode.Img_Sausage);
        foodImageDictionary.Add(EObjectSerialCode.Sausage_Topping, EObjectSerialCode.Img_Sausage);
        //Chocolate,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Chocolate, EObjectSerialCode.Img_Chocolate);
        //Honey,
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Honey, EObjectSerialCode.Img_Honey);

        //Rice,
        foodImageDictionary.Add(EObjectSerialCode.Boiled_Rice, EObjectSerialCode.Img_Rice);
        //Noodle,
        foodImageDictionary.Add(EObjectSerialCode.Boiled_Noodle, EObjectSerialCode.Img_Noodle);

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
        // 수정 필요, FoodImage 매핑 안했음 여기서는 
        GameObject go = FindBySerialCode(serialCode);
        if(typeof(T) == typeof(UnityEngine.UI.Image))
        {
            return Instantiate(go, GameObject.Find("Canvas").transform).GetComponent<T>();
        }
        return Instantiate(go).GetComponent<T>();
    }
}