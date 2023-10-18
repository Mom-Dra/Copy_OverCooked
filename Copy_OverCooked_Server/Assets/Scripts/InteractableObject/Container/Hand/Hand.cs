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

    protected override void Awake()
    {
        base.Awake();
        interactor = GetComponentInChildren<Interactor>();
        player = transform.parent.GetComponent<Player>();
    }

    public void HoldOut()
    {
        getObject.Free();
        getObject.Selectable = true;
        getObject = null;
    }

    public void GrabAndPut()
    {
        InteractableObject triggerObject = interactor.TriggerObject;
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
            if (getObject.TryGetComponent<IFood>(out IFood food))
            {
                Throw();
            } else if (getObject.TryGet<IReactable>(out IReactable reactObject))
            {
                reactObject.React(player);
            }
        } 
        else
        {
            InteractableObject triggerObject = interactor.TriggerObject;
            if (triggerObject != null && triggerObject.TryGetComponent<IReactable>(out IReactable reactObject))
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