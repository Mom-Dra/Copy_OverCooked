using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public interface IStateUIAttachable
{
    public Image StateUI
    {
        get;
        set;
    }

    public void OnProgressBegin();
    public void OnProgressEnd();
}