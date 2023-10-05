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

    private void Awake()
    {
        tcpListener.Start();

        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
    }

    private void TCPConnectCallback(IAsyncResult result)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(result);

        NetworkStream networkStream = client.GetStream();

        string msg = "Hello Client";
        byte[] data = Encoding.ASCII.GetBytes(msg);

        networkStream.Write(data, 0, data.Length);


        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        tcpListener.Stop();
    }
}
