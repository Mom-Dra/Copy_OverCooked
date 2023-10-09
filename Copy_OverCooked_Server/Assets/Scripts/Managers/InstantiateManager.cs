using UnityEngine;
using UnityEngine.UI;

public enum EInGameUIType
{
    Progress,
    Complete,
    Warning,
    Overheat
}

public class InstantiateManager : MonobehaviorSingleton<InstantiateManager>
{
    [Header("InGame UI")]
    [SerializeField]
    private Image progressImage;
    [SerializeField]
    private Image completeImage;
    [SerializeField]
    private Image warningImage;
    [SerializeField]
    private Image overheatImage;

    public Image InstantiateUI(InteractableObject interactableObject, EInGameUIType uIType)
    {
        Image showImage = null;
        switch (uIType)
        {
            case EInGameUIType.Progress:
                showImage = progressImage;
                break;
            case EInGameUIType.Complete:
                showImage = completeImage;
                break;
            case EInGameUIType.Warning:
                showImage = warningImage;
                break;
            case EInGameUIType.Overheat:
                showImage = overheatImage;
                break;
        }
        Vector3 showPos = Camera.main.WorldToScreenPoint(interactableObject.transform.position + interactableObject.uIOffset);
        return Instantiate(showImage, showPos, Quaternion.identity, GameObject.Find("Canvas").transform);
    }
}