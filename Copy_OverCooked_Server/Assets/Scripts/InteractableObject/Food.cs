using UnityEngine;

public class Food : InteractableObject
{
    [Header("Food")]
    public EFoodState foodState;

    public int currCookDegree = 0;

    private void FixedUpdate()
    {
        if (uIImage != null)
        {
            uIImage.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, uIYOffset, 0));
        }
    }

}