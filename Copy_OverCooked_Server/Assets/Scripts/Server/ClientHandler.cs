using System;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class ClientHandler
{
    private TcpClient tcpClient;
    private int clientId;
    private byte[] buffer;

    public ClientHandler(TcpClient tcpClient, int clientId)
    {
        this.tcpClient = tcpClient;
        this.clientId = clientId;

        buffer = new byte[1024];
    }

    public void BeginRead()
    {
        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallback, tcpClient);
    }

    private void ReadCallback(IAsyncResult result)
    {
        NetworkStream stream = tcpClient.GetStream();

        int readLength = stream.EndRead(result);

        if (readLength > 0)
        {
            using (Packet packet = new Packet(buffer.Take(readLength).ToArray()))
            {
                Debug.Log($"<color=magenta> clientId: {clientId}, {packet} </color>");

                PacketHandle.Invoke(packet);
            }
        }

        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallback, tcpClient);
    }

    public void Send(Packet packet)
    {
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(packet.ToByteArray());
    }
}
