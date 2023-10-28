using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Mission", menuName = "Mission")]
public class Mission : ScriptableObject
{
    public Image cookedFoodImage;
    public List<EObjectSerialCode> ingredients = new List<EObjectSerialCode>();
    public List<ECookingMethod> cookingMethods = new List<ECookingMethod>();
}