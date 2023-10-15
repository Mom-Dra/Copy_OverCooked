using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Food : InteractableObject
{
    [Header("Food")]
    [SerializeField]
    private EFoodState foodState;

    [SerializeField]
    private int currCookDegree = 0;

    // Property
    public EFoodState FoodState 
    { 
        get => foodState; 
    }

    public int CurrCookDegree 
    { 
        get => currCookDegree; 
        set 
        { 
            currCookDegree = value; 
        } 
    }

    protected override void Awake()
    {
        base.Awake();
        //if(foodState == EFoodState.Cooked)
        //{
        //    NetworkObject no = GetComponent<NetworkObject>();
        //    Image foodImage = SerialCodeDictionary.Instance.FindFoodImageBySerialCode(no.ObjectSerialCode);
        //    uIComponent.Add(Instantiate(foodImage, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform));
        //}
    }

    private void FixedUpdate()
    {
        if (uIComponent.HasImage)
        {
            uIComponent.OnImagePositionUpdate();
        }
    }

    public Image GetFoodImage()
    {
        NetworkObject no = GetComponent<NetworkObject>();
        return SerialCodeDictionary.Instance.FindFoodImageBySerialCode(no.ObjectSerialCode);
    }

}