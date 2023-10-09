using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text;


public class Server : MonoBehaviour
{
    private TcpListener tcpListener = new TcpListener(IPAddress.Any, 9999);
    private int id;
    private Dictionary<int, ClientHandler> clientDic = new Dictionary<int, ClientHandler>();

    private void Awake()
    {
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        PacketHandler.Init();
    }

    private void TCPConnectCallback(IAsyncResult result)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(result);
        NetworkStream networkStream = client.GetStream();

        ClientHandler clientInfo = new ClientHandler(client, id);

        byte[] sendId = BitConverter.GetBytes(id++);
        networkStream.Write(sendId, 0, sendId.Length);

        clientInfo.BeginRead();

        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
    }

    private void OnApplicationQuit()
    {
        tcpListener.Stop();
    }
}
