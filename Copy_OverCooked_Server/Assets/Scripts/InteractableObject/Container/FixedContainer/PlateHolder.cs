using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateHolder : FixedContainer
{
    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return interactableObject.TryGet<Plate>(out Plate plate) && plate.PlateState == EPlateState.Dirty;
    }

    public void AddPlate(Plate plate)
    {
        plate.gameObject.SetActive(true);
        plate.PlateState = EPlateState.Dirty;
        if(getObject != null && getObject.TryGetComponent<Plate>(out Plate existedPlate))
        {
            existedPlate.TryPut(plate);
        } 
        else
        {
            GetObject = plate;
        }
    }
}
