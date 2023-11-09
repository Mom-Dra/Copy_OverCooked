using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonobehaviorSingleton<LoginManager>
{
    [SerializeField]
    private TMP_InputField hostIPInputField;

    [SerializeField]
    private TMP_InputField userNameInputField;

    [SerializeField]
    private Button joinButton;

    protected override void Awake()
    {
        base.Awake();
        joinButton.onClick.AddListener(OnClickedJoinButton);
    }

    private void OnClickedJoinButton()
    {
        string hostIP = hostIPInputField.text;
        string userName = userNameInputField.text;

        joinButton.interactable = false;

        NetworkManager.Instance.ConnectToServer(hostIP, ConnectSuccessCallBack, ConnectFailCallBack);
    }

    private void ConnectSuccessCallBack()
    {
        SceneManager.LoadScene("InGameClient");
    }

    private void ConnectFailCallBack()
    {
        joinButton.interactable = true;
    }
}
