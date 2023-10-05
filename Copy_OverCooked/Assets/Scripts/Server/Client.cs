using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TcpClient client = new TcpClient("127.0.0.1", 9999);
            Debug.Log("������ ���� �Ǿ����ϴ�.");

            byte[] buffer = new byte[1024];
            int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);

            string serverMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Debug.Log($"������ ���� ���� �޽��� {serverMessage}");

            client.Close();
        }
    }
}
