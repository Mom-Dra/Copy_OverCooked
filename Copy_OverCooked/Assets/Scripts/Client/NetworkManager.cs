using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManager : MonobehaviorSingleton<NetworkManager>
{
    private TcpClient tcpClient;

    [SerializeField]
    private int port = 9999;

    private int clientId;
    private byte[] buffer = new byte[1024];

    //Property
    public int ClientId { get; }

    protected override void Awake()
    {
        base.Awake();
    }

    public void ConnectToServer(string hostIP, UnityAction connectSuccessCallBack, UnityAction connectFailCallBack)
    {
        try
        {
            tcpClient = new TcpClient(hostIP, port);
            Debug.Log("서버에 성공적으로 접속하였습니다!");

            int bytesRead = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
            clientId = BitConverter.ToInt32(buffer, 0);

            Debug.Log($"id: {clientId}");

            connectSuccessCallBack.Invoke();
        }
        catch (Exception e)
        {
            Debug.Log("서버 접속에 실패하였습니다.");
            Debug.LogException(e);
            connectFailCallBack.Invoke();
        }
    }

    public void Send(Packet packet)
    {
        Debug.Log($"<color=magenta> {packet} </color>");

        tcpClient.GetStream().Write(packet.ToByteArray());
    }
}
