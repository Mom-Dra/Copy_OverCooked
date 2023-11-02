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
        Debug.Log($"[{Name}] : ���ð� �ʿ��մϴ�!");
        return false;
    }

    public override void Put(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent<Plate>(out Plate plate))
        {
            if (plate.HasObject() && plate.GetObject.TryGetComponent<IFood>(out IFood iFood))
            {
                Debug.Log($"[{Name}] : �ֹ� ó�� {iFood.GameObject.name}");
                MissionManager.Instance.Submit(iFood.Ingredients);
                Destroy(plate.GetObject.gameObject);
                plate.RemoveSelf();
            }

            Fit(plate);
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