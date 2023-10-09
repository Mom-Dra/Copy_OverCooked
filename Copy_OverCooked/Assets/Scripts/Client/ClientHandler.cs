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
    private int id;
    private byte[] buffer;

    public ClientHandler(TcpClient tcpClient, int id)
    {
        this.tcpClient = tcpClient;
        this.id = id;

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

        if(readLength > 0)
        {
            DecodePacket(readLength);
            //string message = Encoding.UTF8.GetString(buffer, 0, readLength);
            //Debug.Log($"{id}로 부터 받은 메시지: {message}, 크기: {readLength}");
        }

        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallback, tcpClient);
    }

    private void DecodePacket(int readLength)
    {
        using (Packet packet = new Packet(buffer.Take(readLength).ToArray()))
        {
            PacketHandler.Invoke(packet);
        }
    }

    private void Move(Packet packet)
    {
        int targetId = packet.targetId;
        packet.Read(out int direction);
        // Move
    }

    private void Alt()
    {

    }
}
