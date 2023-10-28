using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionBoard : Image
{
    private static readonly List<float[]> imageOffset = new List<float[]>(4)
    {
         new float[1]{ 0f },
         new float[2]{ -25f, 25f },
         new float[3]{ -50f, 0f, 50f },
         new float[4]{ -75f, -25f, 25f, 75f}
    };

    private Mission mission;

    public Mission Mission
    {
        get => mission;
    }

    public void Draw(Mission mission)
    {
        this.mission = mission;
        Image cookedFoodImage = Instantiate(mission.cookedFoodImage, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<Image>();
        cookedFoodImage.transform.SetParent(transform);
        cookedFoodImage.rectTransform.position = transform.position + new Vector3(0f, 75f, 0f);

        Image[] ingredients = new Image[mission.ingredients.Count];
        for(int i = 0; i < ingredients.Length; i++)
        {
            EObjectSerialCode? sc = SerialCodeDictionary.Instance.FindFoodImageSerialCode(mission.ingredients[i]);
            Debug.Log($"org sc : {mission.ingredients[i]}, output sc: {sc}");
            ingredients[i] = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>((EObjectSerialCode)sc);
            ingredients[i].transform.SetParent(transform);
            ingredients[i].transform.position = transform.position + new Vector3(imageOffset[ingredients.Length - 1][i], 0f, 0f);
        }

        if(mission.cookingMethods.Count > 0)
        {
            Image[] cookingMethods = new Image[mission.cookingMethods.Count];
            for (int i = 0; i < cookingMethods.Length; i++)
            {
                cookingMethods[i] = SerialCodeDictionary.Instance.InstantiateByCookingMethod(mission.cookingMethods[i]);
                cookingMethods[i].transform.SetParent(transform);
                cookingMethods[i].transform.position = transform.position + new Vector3(imageOffset[cookingMethods.Length - 1][i], -75f, 0f);
            }
        }
        
    }
}