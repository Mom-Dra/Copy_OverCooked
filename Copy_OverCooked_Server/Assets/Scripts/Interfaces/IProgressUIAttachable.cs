using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public interface IProgressUIAttachable
{
    public Image ProgressImage
    {
        get;
        set;
    }

    public void OnProgressBegin();
    public void OnProgressEnd();
}