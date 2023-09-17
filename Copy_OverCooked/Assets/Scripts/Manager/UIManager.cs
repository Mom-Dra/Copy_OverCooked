using UnityEngine;
using UnityEngine.UI;

public enum EInGameUIType
{
    Progress,
    Complete,
    Warning,
    Overheat
}

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
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

    private void Awake()
    {
        if(Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public Image InstantiateUI(EInGameUIType uIType)
    {
        Image showUI = null;
        switch(uIType)
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
        return Instantiate(showUI, GameObject.Find("Canvas").transform);
    }


}