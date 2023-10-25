using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MixerTray : CookableTray
{
    [Header("Mixer Tray")]
    [SerializeField]
    private float rotateRate = 0.4f;

    private Food mixedPrefab;
    private Food mixed;

    private IEnumerator rotateCoroutine;

    protected override void Awake()
    {
        base.Awake();
        mixedPrefab = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Mixed).GetComponent<Food>();
        rotateCoroutine = RotateCoroutine();
    }

    protected override bool IsValidObject(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            if(iFood.FoodState != EFoodState.Burned)
            {
                return RecipeManager.Instance.TryGetRecipe(CookingMethod, iFood.Ingredients, out Recipe recipe) 
                    && (mixed == null || mixed.Ingredients.Count < maxContainCount);                                                                                       
            }
        }
        return false;
    }

    public override void Put(InteractableObject interactableObject)
    {
        if(interactableObject.TryGetComponent<IFood>(out IFood iFood))
        {
            
            if (mixed == null)
            {
                GetObject = mixed = Instantiate(mixedPrefab);
                mixed.Ingredients.Clear();
            }

            //
            // Mixed 색상 혼합 코드 작성해야함
            //
            if (RecipeManager.Instance.TryGetRecipe(CookingMethod, iFood.Ingredients, out Recipe recipe))
            {
                List<EObjectSerialCode> tmp = new List<EObjectSerialCode>{recipe.CookedFood};
                mixed.Ingredients.AddRange(tmp);
            }
            
            uIComponent.AddRange(iFood.Ingredients);
            ChangeMixedColor(interactableObject);

            Destroy(iFood.GameObject);
        }
    }

    public override void Remove()
    {
        base.Remove();
        mixed = null;
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