using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Interactable Object")]
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string Description;
    [SerializeField]
    protected EObjectType eObjectType;

    [HideInInspector]
    public bool IsInteractable = true;

    public virtual EObjectType GetShownType()
    {
        return eObjectType;
    }

    public virtual bool TryGet<T> (out T result) where T : InteractableObject
    {
        return TryGetComponent<T>(out result);
    }
}