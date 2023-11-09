using UnityEngine;

public class Steamer : CookableTray
{
    private GameObject openLidPrefab;
    private GameObject closeLidPrefab;

    protected override void Awake()
    {
        base.Awake();
        openLidPrefab = transform.Find("Open_Lid").gameObject;
        closeLidPrefab = transform.Find("Close_Lid").gameObject;
    }

    public override void OnProgressBegin()
    {
        closeLidPrefab.SetActive(true);
        openLidPrefab.SetActive(false);
    }

    public override void OnProgressEnd()
    {
        closeLidPrefab.SetActive(false);
        openLidPrefab.SetActive(true);
    }
}