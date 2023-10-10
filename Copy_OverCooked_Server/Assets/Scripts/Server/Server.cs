using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


public class Server : MonobehaviorSingleton<Server>
{
    private TcpListener tcpListener = new TcpListener(IPAddress.Any, 9999);
    private int id;
    private Dictionary<int, ClientHandler> clientDic = new Dictionary<int, ClientHandler>();

    protected override void Awake()
    {
        base.Awake();
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        PacketHandle.Init();
    }

    private void TCPConnectCallback(IAsyncResult result)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(result);
        NetworkStream networkStream = client.GetStream();

        ClientHandler clientHandler = new ClientHandler(client, id);

        byte[] sendId = BitConverter.GetBytes(id++);
        networkStream.Write(sendId, 0, sendId.Length);

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
