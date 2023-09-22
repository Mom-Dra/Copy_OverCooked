using UnityEngine;
using UnityEngine.UI;

public enum EPrefabType
{
    Fire
}

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
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    [Header("InGame UI Images")]
    [SerializeField]
    private Image ProgressBarImage;
    [SerializeField]
    private Image CompleteImage;
    [SerializeField]
    private Image WarningImage;
    [SerializeField]
    private Image OverheatImage;

    [Header("InGame Particles")]
    [SerializeField]
    private GameObject OnFirePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public GameObject InstantiatePrefab(GameObject gameObject, EPrefabType uIType)
    {
        GameObject showPrefab = null;
        switch(uIType)
        {
            case EPrefabType.Fire:
                showPrefab = OnFirePrefab;
                break;
            default:
                break;
        }
        return Instantiate(showPrefab, gameObject.transform.position, Quaternion.identity);
    }

    public Image InstantiateUI(InteractableObject interactableObject, EInGameUIType uIType)
    {
        Image showUI = null;
        switch (uIType)
        {
            case EInGameUIType.Progress:
                showUI = ProgressBarImage;
                break;
            case EInGameUIType.Complete:
                showUI = CompleteImage;
                break;
            case EInGameUIType.Warning:
                showUI = WarningImage;
                break;
            case EInGameUIType.Overheat:
                showUI = OverheatImage;
                break;
            default:
                throw new System.Exception("Invalid UI Image");
        }
        Vector3 instantiatePos = Camera.main.WorldToScreenPoint(gameObject.transform.position) + interactableObject.uIOffset;

        return Instantiate(showUI, instantiatePos,Quaternion.identity, GameObject.Find("Canvas").transform);
    }


}