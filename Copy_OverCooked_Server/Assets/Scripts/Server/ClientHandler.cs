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
        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallback, tcpClient);
    }

    public void Send(Packet packet)
    {
        Debug.Log($"<color=orange> ClientId: {clientId}, {packet}, Length: {packet.GetLength()} </color>");
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(packet.ToByteArray());
    }
}
