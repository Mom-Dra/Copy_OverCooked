using UnityEngine;

public class SettingManager : MonobehaviorSingleton<SettingManager>
{
    [Header("Server")]
    public bool serverEnabled = true;

    [Header("Debug")]
    public bool logEnabled = true;

    [Header("Interactor")]
    public float brigtness = 0.2f;

    [Header("Mission")]
    public bool missionEnabled = true;

    [Header("Fire")]
    public bool fireEnabled = true;


}