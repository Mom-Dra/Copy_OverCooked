using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cookware : FixedContainer
{
    protected ECookwareState cookwareState = ECookwareState.Idle;

    private bool STOP = false;

    protected override void Put(InteractableObject interactableObject)
    {
        base.Put(interactableObject);
    }

    protected bool TryCook()
    {
        if(CanCook())
        {
            StartCoroutine(Cook());
            return true;
        }
        return false;
    }

    protected IEnumerator Cook()
    {
        Debug.Log("Cook!");
        // 실질적으로 조리를 시작하는 코드
        while(true)
        {
            if(STOP)
            {
                break;
            }
            Debug.Log("Cooking...");
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Cook Complete!");
    }

    public void StopCook()
    {
        STOP = true;
    }

    protected abstract bool CanCook();
}
