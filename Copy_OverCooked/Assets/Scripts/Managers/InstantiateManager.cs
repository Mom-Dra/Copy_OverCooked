using UnityEngine;
using UnityEngine.UI;

public enum EInGameUIType
{
    Progress,
    Complete,
    Warning,
    Overheat
}

public class InstantiateManager : MonoBehaviour
{
    private static InstantiateManager instance;
    public static InstantiateManager Instance
    {
        get => instance;
    }

    [Header("InGame UI")]
    [SerializeField]
    private Image progressImage;
    [SerializeField]
    private Image completeImage;
    [SerializeField]
    private Image warningImage;
    [SerializeField]
    private Image overheatImage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        Vector3 showPos = Camera.main.WorldToScreenPoint(interactableObject.transform.position + new Vector3(0, interactableObject.uIYOffset, 0));
        return Instantiate(showImage, showPos, Quaternion.identity, GameObject.Find("Canvas").transform);
    }
}