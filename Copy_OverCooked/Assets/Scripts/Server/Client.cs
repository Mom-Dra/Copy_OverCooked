using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    private TcpClient tcpClient;

    [SerializeField]
    private string hostIP = "127.0.0.1";

    [SerializeField]
    private int port = 9999;

    private int id;
    private byte[] buffer = new byte[1024];

    [SerializeField]
    private string msg;

    private void Start()
    {
        tcpClient = new TcpClient(hostIP, port);
        Debug.Log("서버에 연결 되었습니다.");

        int bytesRead = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
        id = BitConverter.ToInt32(buffer, 0);

        Debug.Log($"id: {id}");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {

        }
        else if (Input.GetKeyUp(KeyCode.A))
        {

        }
        else if (Input.GetKeyUp(KeyCode.D))
        {

        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(msg);
            tcpClient.GetStream().Write(sendBytes);
        }
    }

    private void Move(int direction)
    {
        Packet packet = new Packet(id, EActionCode.Input, ETargetType.Player, direction);
        tcpClient.GetStream().Write(packet.ToByteArr());
    }
}
