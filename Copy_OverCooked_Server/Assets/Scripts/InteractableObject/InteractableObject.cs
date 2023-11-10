using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class InteractableObject : SerializedObject
{
    [Header("Interactable Object")]
    public string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected EObjectType objectType;
    [SerializeField]
    protected Vector2 uIOffset = new Vector2 (0f, 75f);

    // 이거 일단 이렇게 하고 추후 수정 
    // 
    public Interactor inclusiveInteractor;

    protected Renderer[] renderers;

    protected bool selectable = true;
    protected bool onGlowShader = false;
            
    // Property
    public bool Selectable 
    {      
        get => selectable; 
        set 
        {  
            selectable = value; 
        }  
    }

    public Vector3 UIOffset 
    {
        get => uIOffset; 
    }

    public EObjectType ObjectType
    {
        get => objectType;
    }

    [ContextMenu("Initialize")]
    private void SetSerialCode()
    {
        Name = name;
        if (Enum.TryParse(Name, out EObjectSerialCode sc))
        {
            serialCode = sc;
            int scToInt = (int)serialCode;
            if((Name.Equals("Chopped_Dough") || Name.Equals("Tortilla")) || (scToInt > 0 && scToInt < 20))
            {
                objectType = EObjectType.Tray;
            }else if(scToInt >= 20 && scToInt < 30)
            {
                objectType = EObjectType.Other;
            }else if(scToInt >= 30 && scToInt < 100)
            {
                objectType = EObjectType.Empty_Fixed_Container;
            }else  if(scToInt >= 100 && scToInt < 400)
            {
                objectType = EObjectType.Food;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        renderers = gameObject.GetComponentsInAllChildren<Renderer>().ToArray();
        //List<Renderer> getRendererList = new List<Renderer>();
        //Renderer myRenderer = GetComponent<Renderer>();
        //if(myRenderer != null)
        //{
        //    getRendererList.Add(myRenderer);
        //}
        //Renderer[] getRenderers = GetComponentsInChildren<Renderer>();

        //if(getRenderers != null)
        //{
        //    getRendererList.AddRange(getRenderers);
        //}

        //renderers = getRendererList.ToArray();
    }

    public virtual EObjectType GetTopType()
    {
        return objectType;
    }


    public virtual bool TryGet<T>(out T result, EGetMode getMode = EGetMode.Peek)
    {
        return TryGetComponent<T>(out result);
    }

    public virtual void GlowOn()
    {
        SetBrightnessInRenderers(SettingManager.Instance.brigtness);
        onGlowShader = true;
    }

    public virtual void GlowOff()
    {
        SetBrightnessInRenderers(0f);
        onGlowShader = false;
    }

    protected void SetBrightnessInRenderers(float amount)
    {
        if(renderers != null)
        {
            foreach(Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_Brightness", amount);
            }
        }
    }

    protected void SetColorInRenderers(Color color)
    {
        if(renderers != null)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetColor("_Color", color);
            }
        }
    }

    public Renderer GetFirstRenderer()
    {
        return renderers[0];
    }

    protected virtual void OnDestroy()
    {
        if(inclusiveInteractor != null)
        {
            inclusiveInteractor.RemoveOnDestroy(this);
        }
    }


}