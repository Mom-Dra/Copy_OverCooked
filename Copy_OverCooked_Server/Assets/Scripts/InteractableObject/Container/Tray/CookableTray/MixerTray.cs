using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MixerTray : CookableTray
{
    [Header("Mixer Tray")]
    [SerializeField]
    private float rotateRate = 0.4f;

    private Mixed mixedPrefab;
    private Mixed mixed;

    private IEnumerator rotateCoroutine;

    protected override void Awake()
    {
        base.Awake();
        mixedPrefab = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Mixed).GetComponent<Mixed>();
        rotateCoroutine = RotateCoroutine();
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            if(iFood.FoodState != EFoodState.Burned)
            {
                if(iFood.CookingMethod == CookingMethod || RecipeManager.Instance.TryGetRecipe(CookingMethod, iFood.Ingredients, out Recipe recipe))
                {
                    return (mixed == null || mixed.Ingredients.Count + iFood.Ingredients.Count <= maxContainCount);
                }                                                                                  
            }
        }
        return false;
    }

    public override void Put(InteractableObject interactableObject)
    {
        if (interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            uIComponent.AddRange(iFood.Ingredients);

            if (interactableObject.TryGetComponent<Mixed>(out Mixed putMixed))
            {
                if (mixed == null)
                {
                    GetObject = mixed = putMixed;
                } 
                else
                {
                    mixed.CurrCookingRate += putMixed.CurrCookingRate;
                    mixed.Ingredients.AddRange(putMixed.Ingredients);
                    mixed.CurrOverTime = 0;
                    Destroy(putMixed.GameObject);
                }
            } 
            else 
            {
                if (mixed == null)
                {
                    GetObject = mixed = Instantiate(mixedPrefab);
                    mixed.Ingredients.Clear();
                }

                if (RecipeManager.Instance.TryGetRecipe(CookingMethod, iFood.Ingredients, out Recipe recipe))
                {
                    List<EObjectSerialCode> tmp = new List<EObjectSerialCode> { recipe.CookedFood };
                    mixed.Ingredients.AddRange(tmp);
                    mixed.CurrOverTime = 0;
                    Destroy(iFood.GameObject);
                }
            }
            
            if (mixed.CurrCookingRate > 0 && mixed.CurrOverTime == 0)
            {
                if (progressImage != null && progressImage.TryGetComponent<SerializedObject>(out SerializedObject so))
                {
                    if (so.SerialCode != EObjectSerialCode.Img_Progress)
                    {
                        Destroy(progressImage.gameObject);
                        ProgressImage = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Progress);
                    }
                } 
                else
                {
                    ProgressImage = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Progress);
                }
                Image gauge = progressImage.transform.GetChild(1).GetComponent<Image>();
                gauge.fillAmount = mixed.CurrCookingRate / (parentCookware.TotalCookDuration + mixed.Ingredients.Count * 4f);
            }


            //
            ChangeMixedColor(interactableObject);

            
        }
    }

    public override void Remove(InteractableObject interactableObject)
    {
        base.Remove(interactableObject);
        mixed = null;
    }

    private void AddCookDuration(IFood iFood)
    {
        mixed.CurrCookingRate = (mixed.CurrCookingRate < iFood.CurrCookingRate) ? mixed.CurrCookingRate : iFood.CurrCookingRate;
        mixed.CurrOverTime = 0;
    }

    private void ChangeMixedColor(InteractableObject io)
    {
        // 현재 Mixed 색상과 새로 들어온 음식의 색상을 합침 
    }

    public override void OnProgressBegin()
    {
        StartCoroutine(rotateCoroutine);
    }

    public override void OnProgressEnd()
    {
        StopCoroutine(rotateCoroutine);
    }


    private IEnumerator RotateCoroutine()
    {
        while(true)
        {
            transform.Rotate(0f, rotateRate, 0f);
            yield return null;
        }
    }
}