using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class Server : MonobehaviorSingleton<Server>
{
    [SerializeField]
    private int clientNumberForDebug = 0; // Just Debug

    private TcpListener tcpListener = new TcpListener(IPAddress.Any, 9999);
    private int s_nextId;
    private Dictionary<int, TCPClientHandler> clientDic = new Dictionary<int, TCPClientHandler>();

    private UdpClient udpClient = new UdpClient(9999);

    protected override void Awake()
    {
        base.Awake();
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        PacketHandle.Init();
        Debug.Log("������ ���۵Ǿ����ϴ�.");

        UDP();
    }

    #region TCP
    private void TCPConnectCallback(IAsyncResult result)
    {
        NetworkDebug.Log($"Hello [{s_nextId}] User!");
        TcpClient client = tcpListener.EndAcceptTcpClient(result);
        NetworkStream networkStream = client.GetStream();

        TCPClientHandler clientHandler = new TCPClientHandler(client, s_nextId);
        clientDic.Add(s_nextId, clientHandler);
        clientNumberForDebug++;

        byte[] sendId = BitConverter.GetBytes(s_nextId);
        networkStream.Write(sendId, 0, sendId.Length);

        int ID = s_nextId;
        UnityMainThread.Instance.AddJob(() =>
        {
            NetworkObjectManager.Instance.SpawnPlayer(ID);
        });

        s_nextId++;

        clientHandler.BeginRead();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
    }

    public void SendToClient(Packet packet, int clientId)
    {
        clientDic[clientId].Send(packet);
    }

    public void SendToAllClients(Packet packet)
    {
        foreach (TCPClientHandler clientHandler in clientDic.Values)
        {
            clientHandler.Send(packet);
        }
    }

    #endregion

    private void UDP()
    {
        //udpClient.BeginReceive();

        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        byte[] data = udpClient.Receive(ref remoteEP);
        string message = Encoding.UTF8.GetString(data);

        Debug.Log($"From Client: {message}");
    }


    private void OnApplicationQuit()
    {
        tcpListener.Stop();
    }
}
