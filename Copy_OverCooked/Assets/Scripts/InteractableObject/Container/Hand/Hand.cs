using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.XR;

public class Hand : Container
{
    [SerializeField]
    private Interactor interactor;

    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + containOffset;
        }
    }

    private void Awake()
    {
        interactor = GetComponentInChildren<Interactor>();
    }
    public void HoldOut()
    {
        getObject.Free();
        getObject.IsInteractable = true;
        getObject = null;
    }

    public void GrabAndPut()
    {
        InteractableObject triggerObject = interactor.ClosestInteractableObject;
        if (HasObject() && triggerObject == null)
        {
            HoldOut();
            return;
        }
        InteractManager.Instance.Match(this, triggerObject);
    }

}