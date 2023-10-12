using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientHandler
{
    private TcpClient tcpClient;
    private int cliendId;
    private byte[] buffer;

    public ClientHandler(TcpClient tcpClient, int id)
    {
        this.tcpClient = tcpClient;
        this.cliendId = id;

        buffer = new byte[1024];
    }

    public void BeginRead()
    {
        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallback, tcpClient);
    }

    // 이거 안쓰는거 아닌가 
    private void ReadCallback(IAsyncResult result)
    {
        NetworkStream stream = tcpClient.GetStream();

        int readLength = stream.EndRead(result);

        if(readLength > 0)
        {
            using (Packet packet = new Packet(buffer.Take(readLength).ToArray()))
            {
                Debug.Log($"{packet}");
                PacketHandle.Invoke(packet);
            }
        }

        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallback, tcpClient);
    }

    private void Alt()
    {

    }
}
