using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


public class Server : MonobehaviorSingleton<Server>
{
    [SerializeField]
    private int clientNumberForDebug  = 0; // Just Debug

    private TcpListener tcpListener = new TcpListener(IPAddress.Any, 9999);
    private int s_nextId;
    private Dictionary<int, ClientHandler> clientDic = new Dictionary<int, ClientHandler>();


    protected override void Awake()
    {
        base.Awake();
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        PacketHandle.Init();

        Debug.Log("서버가 시작되었습니다.");
    }

    private void TCPConnectCallback(IAsyncResult result)
    {
        NetworkDebug.Log($"Hello [{s_nextId}] User!");
        TcpClient client = tcpListener.EndAcceptTcpClient(result);
        NetworkStream networkStream = client.GetStream();

        ClientHandler clientHandler = new ClientHandler(client, s_nextId);
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
        foreach (ClientHandler clientHandler in clientDic.Values)
        {
            clientHandler.Send(packet);
        }
    }

    private void OnApplicationQuit()
    {
        tcpListener.Stop();
    }
}
