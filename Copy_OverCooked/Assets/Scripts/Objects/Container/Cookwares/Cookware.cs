using System.Collections;
using UnityEngine;

public enum ECookwareState
{
    Idle,
    Cook,
    Complete,
    Overheat
}

public abstract class Cookware : Container
{
    [SerializeField]
    protected CookingMethod cookingMethod;
    [SerializeField]
    private bool IsImmediateCook = true;

    protected ECookwareState cookwareState;

    protected float currProgressTime;

    private bool STOP = false;

    private void Awake()
    {
        cookwareState = ECookwareState.Idle;
    }

    private IEnumerator Cook()
    {
        // ���� �������� ����� �ٸ� 
        // ������ �������� ������ ���� ���� ���� = Get() �Լ� override �ؾ��� �� 
        Recipe recipe = RecipeManager.Instance.Search(cookingMethod, containObjects);
        Debug.Log(recipe);
        float cookDuration = recipe.getCookTime();
        currProgressTime = 0;

        while (currProgressTime < cookDuration)
        {
            if (STOP)
                yield break;

            currProgressTime += 0.1f;
            Debug.Log($"Progress: {currProgressTime} / {cookDuration}%");
            yield return new WaitForSeconds(0.1f);
        }

        containObjects.Clear();
        //getObject.RemoveFromInteractor();
        LinkManager.Instance.GetLinkedObject(this).GetInteractor().RemoveObject(getObject);
        Destroy(getObject.gameObject);

        getObject = Instantiate(recipe.getCookedFood().gameObject, transform.position + offset, Quaternion.identity).GetComponent<InteractableObject>();
        if (getObject == null)
        {
            Debug.Log("Invalid Component : 'Food'");
        } else
        {
            cookwareState = ECookwareState.Complete;
        }
        CompletedCook();
        LinkManager.Instance.Disconnect(this);
    }

    protected void StartCook()
    {
        STOP = false;
        cookwareState = ECookwareState.Cook;
        StartCoroutine(Cook());
    }

    protected void StopCook()
    {
        STOP = true;
    }

    public virtual bool Interact()
    {
        if (cookwareState != ECookwareState.Complete)
        {
            StartCook();
            return true;
        }
        return false;
    }

    public override bool Put(InteractableObject gameObject)
    {
        if (base.Put(gameObject))
        {
            if (IsImmediateCook && IsFull())
            {
                StartCook();
            }
            return true;
        }
        return false;
    }

    protected abstract void CompletedCook();
}
