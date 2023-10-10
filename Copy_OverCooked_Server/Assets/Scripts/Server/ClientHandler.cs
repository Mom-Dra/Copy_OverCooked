using System;
using System.Linq;
using System.Net.Sockets;

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

        if (readLength > 0)
        {
            using (Packet packet = new Packet(buffer.Take(readLength).ToArray()))
            {
                PacketHandler.Invoke(packet);
            }
            //string message = Encoding.UTF8.GetString(buffer, 0, readLength);
            //Debug.Log($"{id}로 부터 받은 메시지: {message}, 크기: {readLength}");
        }

        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, ReadCallback, tcpClient);
    }

    public void Send(Packet packet)
    {
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(packet.ToByteArray());
    }
}
