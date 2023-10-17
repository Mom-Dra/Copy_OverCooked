using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Plate : Tray
{
    [SerializeField]
    private EPlateState plateState = EPlateState.Clean;

    private Renderer plateRenderer;
    private Color cleanColor;

    public EPlateState PlateState
    {
        get 
        { 
            return plateState; 
        }
        set
        {
            if(plateState != value)
            {
                OnPlateStateChanging(value);
            }
            plateState = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        plateRenderer = GetComponent<Renderer>();
        cleanColor = plateRenderer.material.color;
    }

    private void OnPlateStateChanging(EPlateState plateState)
    {
        if(plateState == EPlateState.Clean)
        {
            plateRenderer.material.color = cleanColor;
        } 
        else
        {
            plateRenderer.material.color = Color.gray;
        }
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if(plateState == EPlateState.Clean)
        {
            return base.IsValidObject(interactableObject);
        }
        return interactableObject.TryGet<Plate>(out Plate plate) && plate.PlateState == EPlateState.Dirty;
    }

    //public override bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek)
    //{
    //    result = default(T);
    //    if(typeof(T) == typeof(Tray) && plateState == EPlateState.Dirty)
    //    {
    //        result = GetComponent<T>();
    //    } 
    //    else
    //    {
    //        base.TryGet<T>(out result, getMode);
    //    }

    //    return result != null;
    //}

    public override void Put(InteractableObject interactableObject)
    {
        if(plateState == EPlateState.Clean)
        {
            base.Put(interactableObject);
        } 
        else
        {
            StackPlate(interactableObject);
        }
    }

    public void StackPlate(InteractableObject interactableObject)
    {
        if (getObject != null && TryGet<Plate>(out Plate plate))
        {
            plate.Put(interactableObject);
        } else
        {
            GetObject = interactableObject;
        }
    }

    public override void Remove()
    {
        base.Remove();
    }
}