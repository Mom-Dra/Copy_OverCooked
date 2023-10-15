using UnityEngine;
using UnityEngine.UI;

public enum EInGameUIType
{
    Progress,
    Complete,
    Warning,
    Overheat,
    PlusBase
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
    [SerializeField]
    private Image plusBaseImage;

    public Image InstantiateByUIType(InteractableObject interactableObject, EInGameUIType uIType)
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
            case EInGameUIType.PlusBase:
                showImage = plusBaseImage;
                break;
        }
        Vector3 showPos = Camera.main.WorldToScreenPoint(interactableObject.transform.position + interactableObject.UIOffset);
        return Instantiate(showImage, showPos, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    public Image InstantiateByUIType(EInGameUIType uIType)
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
            case EInGameUIType.PlusBase:
                showImage = plusBaseImage;
                break;
        }
        return InstantiateOnCanvas(showImage);
    }

    public Image InstantiateOnCanvas(Image go)
    {
        return Instantiate(go, GameObject.Find("Canvas").transform).GetComponent<Image>();
    }
}