using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class OrderOutTable : FixedContainer
{
    [Header("Order Out Table")]
    [SerializeField]
    private PlateHolder plateHolder;

    private static readonly WaitForSeconds waitForReturnPlate = new WaitForSeconds(5f);

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<Plate>(out Plate plate))
        {
            return true;
        } 
        else
        {
            Debug.Log($"[{Name}] : 접시가 필요합니다!");
        }
        return false;
    }

    public override void Put(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent<Plate>(out Plate plate))
        {
            if (plate.HasObject())
            {
                Debug.Log($"[{Name}] : 주문 처리 {plate.GetObject}");
                Destroy(plate.GetObject.gameObject);
                plate.Remove();
            }
            plate.gameObject.SetActive(false);
            StartCoroutine(ReturnPlateCoroutine(plate));
        }
    }

    private IEnumerator ReturnPlateCoroutine(Plate plate)
    {
        yield return waitForReturnPlate;
        plateHolder.AddPlate(plate);
    }

    
}