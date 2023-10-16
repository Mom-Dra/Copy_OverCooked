using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tray : Container
{
    [Header("Tray")]
    [SerializeField]
    private bool PlusBaseUI = true;

    protected override void Awake()
    {
        if (PlusBaseUI)
        {
            uIComponent = new BaseUIComponent(transform, uIOffset, maxContainCount);
        }
        base.Awake();
    }

    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + displayOffset;
        }
        if (uIComponent.HasImage)
        {
            uIComponent.OnImagePositionUpdate();
        }
    }

    public override void Put(SendObjectArgs sendContainerArgs)
    {
        if(sendContainerArgs.Item.UIComponent.HasImage )
        {
            sendContainerArgs.Item.UIComponent.Clear();
        }

        if(sendContainerArgs.ContainObjects.Count + containObjects.Count == 1)
        {
            base.Put(sendContainerArgs);
            return;
        }

        if (TryGetCombinedRecipe(sendContainerArgs.ContainObjects.Concat(containObjects).ToList(), out Recipe recipe))
        {
            if (recipe.MainImage != null)
            {
                uIComponent.Clear();
                uIComponent.AddInstantiate(recipe.MainImage); 
            } 
            else
            {
                foreach(EObjectSerialCode serialCode in sendContainerArgs.ContainObjects)
                {
                    uIComponent.Add(serialCode);
                }
            }
            using (SendObjectArgs args = new SendObjectArgs(Instantiate(recipe.CookedFood), sendContainerArgs.ContainObjects))
            {
                base.Put(args);
            }
        }
        //Food food = sendContainerArgs.Item as Food;
        //uIComponent.Add(InstantiateManager.Instance.InstantiateOnCanvas(food.GetFoodImage()));
    }

    protected override bool IsValidObject(List<EObjectSerialCode> serialObjects)
    {
        if (base.IsValidObject(serialObjects))
        {
            if(serialObjects.Count == 1 )
            {
                return true;
            }
            return TryGetCombinedRecipe(serialObjects, out Recipe recipe);
        }
        return false;
    }

    protected bool TryGetCombinedRecipe(List<EObjectSerialCode> serialObjects, out Recipe recipe)
    {
        recipe = null;
        RecipeManager.Instance.TryGetRecipe(ECookingMethod.Combine, serialObjects, out recipe);
        return recipe != null;
    }
}
