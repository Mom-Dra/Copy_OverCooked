using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManager : MonobehaviorSingleton<NetworkManager>
{
    private TcpClient tcpClient;

    [SerializeField]
    private int port = 9999;

    private int clientId = -1;
    private byte[] buffer = new byte[1024];

    //Property
    public int ClientId { get; }

    protected override void Awake()
    {
        base.Awake();
        PacketHandle.Init();
    }

    public void ConnectToServer(string hostIP, UnityAction connectSuccessCallBack, UnityAction connectFailCallBack)
    {
        try
        {
            tcpClient = new TcpClient(hostIP, port);
            NetworkDebug.Log("서버에 성공적으로 접속하였습니다!");

            int bytesRead = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
            clientId = BitConverter.ToInt32(buffer, 0);

            NetworkDebug.Log($"id: {clientId}");

            connectSuccessCallBack.Invoke();


            tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallBack, tcpClient);
        }
        catch (Exception e)
        {
            NetworkDebug.Log("서버 접속에 실패하였습니다.");
            Debug.LogException(e);
            connectFailCallBack.Invoke();
        }
    }

    private void ReadCallBack(IAsyncResult result)
    {
        NetworkStream stream = tcpClient.GetStream();

        int readLength = stream.EndRead(result);
        int readPos = 0;
        while (readPos < readLength)
        {
            int packetLength = BitConverter.ToInt32(buffer, readPos);
            readPos += 4;

            if (packetLength > 0)
            {
                byte[] packetUnitData = buffer.Skip(readPos).Take(packetLength).ToArray();
                UnityMainThread.Instance.AddJob(() =>
                {
                    int readPosForDelay = readPos;
                    using (Packet packet = new Packet(packetUnitData))
                    {
                        Debug.Log($"<color=yellow> {packet} </color>");
                        PacketHandle.Invoke(packet);
                    }
                });
                readPos += packetLength;
            } else
                break;
        }

        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallBack, tcpClient);
    }

    public void Send(Packet packet)
    {
        Debug.Log($"<color=yellow> {packet} </color>");
        tcpClient.GetStream().Write(packet.ToByteArray());
    }
}
