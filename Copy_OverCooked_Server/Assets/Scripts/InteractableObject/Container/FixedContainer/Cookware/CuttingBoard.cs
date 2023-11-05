using UnityEngine;

public class CuttingBoard : Cookware, IReactable
{
    private GameObject knifePrefab;

    protected override void Awake()
    {
        base.Awake();
        knifePrefab = transform.Find("KnifePrefab").gameObject;
    }

    public void React(Player player)
    {
        if (TryCook())
        {
            EventManager.Instance.AddEvent(new ChopEvent(player, this));
        }
    }

    protected override bool CanCook()
    {
        return true;
    }

    protected override bool CanGet()
    {
        IFood getFood = getObject as IFood;
        return cookwareState != ECookwareState.Cook || (cookwareState == ECookwareState.Cook && getFood.CurrCookingRate < 5);
    }

    public override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
        knifePrefab.SetActive(false);
    }

    public override void Remove(InteractableObject interactableObject)
    {
        base.Remove(interactableObject);
        knifePrefab.SetActive(true);
    }


    public override void OnProgressBegin()
    {
        
    }

    public override void OnProgressEnd()
    {

    }
}