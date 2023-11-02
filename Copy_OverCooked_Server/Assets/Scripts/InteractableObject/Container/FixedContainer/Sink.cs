using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.Analytics;
using Unity.VisualScripting.Dependencies.NCalc;

public class Sink : FixedContainer, IProgressUIAttachable, IReactable
{
    private static readonly WaitForSeconds waitForTick = new WaitForSeconds(0.1f);

    [Header("Sink")]
    [SerializeField]
    private float washingTime = 2f;
    [SerializeField]
    private Stack<Plate> dirtyPlateStack = new Stack<Plate>();

    private GameObject hasDirtyPlatePrefab;

    private Image stateImage;

    private IEnumerator selectedCoroutine;

    private Stack<Plate> cleanPlateStack = new Stack<Plate>();

    private float currWashingTime = 0f;

    private Container PutContainer
    {
        get
        {
            return cleanPlateStack.Count > 0 ? cleanPlateStack.Peek() : this;
        }
    }

    public Image ProgressImage
    {
        get
        {
            return stateImage;
        }
        set
        {
            stateImage = value;
            if (stateImage != null)
            {
                stateImage.transform.position = Camera.main.WorldToScreenPoint(transform.position) + UIOffset;
            }
        }
    }

    public bool HasDirtyPlate
    {
        get
        {
            return dirtyPlateStack.Count > 0;
        }
    }

    private Plate DirtyPlate
    {
        get
        {
            Plate plate = dirtyPlateStack.Pop();
            plate.gameObject.SetActive(true);

            if (dirtyPlateStack.Count == 0)
            {
                hasDirtyPlatePrefab.gameObject.SetActive(false);
            }

            return plate;
        }
        set
        {
            value.gameObject.SetActive(false);
            dirtyPlateStack.Push(value);

            if(dirtyPlateStack.Count > 0)
            {
                hasDirtyPlatePrefab.gameObject.SetActive(true);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        hasDirtyPlatePrefab = transform.Find("HasDirtyPlate").gameObject;
    }

    public void React(Player player)
    {
        if (dirtyPlateStack.Count > 0)
        {
            EventManager.Instance.AddEvent(new DishwashingEvent(player, this));
            selectedCoroutine = WashingCoroutine();
            StartCoroutine(selectedCoroutine);
        }
    }

    private IEnumerator WashingCoroutine()
    {
        if (stateImage == null)
        {
            ProgressImage = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Progress);
        }
        Image gauge = stateImage.transform.GetChild(1).GetComponent<Image>();


        while(currWashingTime  < washingTime)
        {
            currWashingTime += 0.1f;
            gauge.fillAmount = currWashingTime / washingTime;
            yield return waitForTick;
        }

        currWashingTime = 0f;

        Plate popPlate = DirtyPlate;

        popPlate.PlateState = EPlateState.Clean;
        PutContainer.GetObject = popPlate;
        cleanPlateStack.Push(popPlate);

        if (dirtyPlateStack.Count == 0)
        {
            selectedCoroutine = null;
            Destroy(stateImage.gameObject);
            stateImage = null;
        } 
        else
        {
            selectedCoroutine = WashingCoroutine();
            StartCoroutine(selectedCoroutine);
        }
    }

    public void StopSelectedCoroutine()
    {
        if(selectedCoroutine != null)
        {
            StopCoroutine(selectedCoroutine);
        }
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        return interactableObject.TryGet<Plate>(out Plate plate) && plate.PlateState == EPlateState.Dirty;
    }

    public override void Put(InteractableObject interactableObject)
    {
        if(interactableObject.TryGet(out Plate plate))
        {
            Plate currPlate = plate;
            Plate prevPlate = null;

            DirtyPlate = currPlate;

            while (currPlate.HasObject())
            {
                prevPlate = currPlate;
                currPlate = currPlate.GetObject as Plate;
                prevPlate.RemoveSelf();
                DirtyPlate = currPlate; 
            }
        }
    }

    public override bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek)
    {
        result = default(T);
        if (HasObject())
        {
            result = cleanPlateStack.Peek().GetComponent<T>();
        }

        return result != null;
    }

    public override void Remove(InteractableObject interactableObject)
    {
        base.Remove(interactableObject);
        cleanPlateStack.Pop();
        if (cleanPlateStack.Count > 0)
        {
            cleanPlateStack.Peek().RemoveSelf();
        }
    }

    public void OnProgressBegin()
    {
        
    }

    public void OnProgressEnd()
    {
        
    }
}