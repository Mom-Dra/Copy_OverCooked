using System.Collections.Generic;
using System.Threading;
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
        SerializedObject[] load_SerializedObject = Resources.LoadAll<SerializedObject>("Prefabs/");
        foreach (SerializedObject obj in load_SerializedObject)
        {
            Add(obj.SerialCode, obj.gameObject);
        }

        MatchSerialCodeToFoodImage();
    }

    private void Add(EObjectSerialCode serialCode, GameObject prefab)
    {
        serialCodDic.Add(serialCode, prefab);
    }
    

    private void MatchSerialCodeToFoodImage()
    {
        //Bread,
        foodImageDictionary.Add(EObjectSerialCode.Bread, EObjectSerialCode.Img_Bread);
        //Seaweed,
        foodImageDictionary.Add(EObjectSerialCode.Seaweed, EObjectSerialCode.Img_Seaweed);
        //Flour,
        foodImageDictionary.Add(EObjectSerialCode.Flour, EObjectSerialCode.Img_Flour);
        //Tortilla,
        foodImageDictionary.Add(EObjectSerialCode.Tortilla, EObjectSerialCode.Img_Tortilla);
        //Egg,
        foodImageDictionary.Add(EObjectSerialCode.Egg, EObjectSerialCode.Img_Egg);


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
        foodImageDictionary.Add(EObjectSerialCode.Chopped_Pineapple, EObjectSerialCode.Img_Pineapple);
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
        foodImageDictionary.Add(EObjectSerialCode.Rice, EObjectSerialCode.Img_Rice);
        foodImageDictionary.Add(EObjectSerialCode.Boiled_Rice, EObjectSerialCode.Img_Rice);
        //Noodle,
        foodImageDictionary.Add(EObjectSerialCode.Noodle, EObjectSerialCode.Img_Noodle);
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