using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.XR;

public class Hand : Container
{
    [Header("Hand")]
    [SerializeField]
    private float throwPower = 6f;

    private Player player;
    private Interactor interactor;


    private void FixedUpdate()
    {
        if (getObject != null)
        {
            getObject.transform.position = transform.position + displayOffset;
        }
    }

    private void Awake()
    {
        interactor = GetComponentInChildren<Interactor>();
        player = transform.parent.GetComponent<Player>();
    }

    public void HoldOut()
    {
        getObject.Free();
        getObject.IsInteractable = true;
        containObjects.Remove(getObject);
        getObject = null;
    }

    public void GrabAndPut()
    {
        Debug.Log("Space");
        InteractableObject triggerObject = interactor.ClosestInteractableObject;
        if (triggerObject == null)
        {
            if (HasObject())
            {
                HoldOut();
            }
            return;
        }
        InteractManager.Instance.Match(this, triggerObject);
    }

    public void InteractAndThrow()
    {
        if (HasObject())
        {
            if (getObject.TryGetComponent<Food>(out Food food))
            {
                Throw();
            } else if (getObject.TryGet<Reactable>(out Reactable reactObject))
            {
                reactObject.React(player);
            }
        } else
        {
            InteractableObject triggerObject = interactor.ClosestInteractableObject;
            if (triggerObject != null && triggerObject.TryGet<Reactable>(out Reactable reactObject))
            {
                reactObject.React(player);
            }
        }
    }

    private void Throw()
    {
        InteractableObject throwObject = getObject;
        HoldOut();
        throwObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwPower, ForceMode.Impulse);
    }
}