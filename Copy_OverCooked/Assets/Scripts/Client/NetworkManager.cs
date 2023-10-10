using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkManager : MonobehaviorSingleton<NetworkManager>
{
    private TcpClient tcpClient;

    [SerializeField]
    private string hostIP = "127.0.0.1";

    [SerializeField]
    private int port = 9999;

    private int clientId;
    private byte[] buffer = new byte[1024];

    [SerializeField]
    private string msg;

    protected override void Awake()
    {
        base.Awake();
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        tcpClient = new TcpClient(hostIP, port);
        Debug.Log("서버에 연결 되었습니다.");

        int bytesRead = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
        clientId = BitConverter.ToInt32(buffer, 0);

        Debug.Log($"id: {clientId}");
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(msg);
            tcpClient.GetStream().Write(sendBytes);
        }
    }

    public void Move(Vector2 vector2)
    {
        using(Packet packet = new Packet(EActionCode.Input, clientId))
        {
            packet.Write((int)EInputType.Move);
            packet.Write(clientId);
            packet.Write(vector2);
            Send(packet);
        }
    }

    private void Send(Packet packet)
    {
        tcpClient.GetStream().Write(packet.ToByteArray());
    }
}
