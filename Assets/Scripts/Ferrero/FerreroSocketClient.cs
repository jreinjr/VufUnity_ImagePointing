using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class FerreroSocketClient : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    string serverIp = "127.0.0.1";
    int port = 65432;
    int sendFPS = 144;
    float sendDelay;
    public byte[] data;
    void Start()
    {
        // Create some data to send (48x80 bytes)
        data = new byte[3840];
        //// Fill data with some content (e.g., random values)
        //new System.Random().NextBytes(data);

        sendDelay = 1f / (float)sendFPS;
        client = new TcpClient(serverIp, port);  // Connect to server
        stream = client.GetStream();

        //StartCoroutine(SendDataLoop());
    }

    public void SendData(byte[] data)
    {
        if (stream == null || client == null) return;
        if (stream.CanWrite)
        {
            // Send data to the server
            stream.Write(data, 0, data.Length);
        }
    }

    IEnumerator SendDataLoop()
    {
        while (true)
        {

            Debug.Log(data.Length);
            if (stream.CanWrite)
            {
                // Send data to the server
                stream.Write(data, 0, data.Length);
            }

            yield return new WaitForSeconds(sendDelay);

        }
    }

    void Update()
    {
        //// Create some data to send (48x80 bytes)
        //byte[] data = new byte[3840];
        //// Fill data with some content (e.g., random values)
        //new System.Random().NextBytes(data);
        //Debug.Log(data.Length);
        //if (stream.CanWrite)
        //{
        //    // Send data to the server
        //    stream.Write(data, 0, data.Length);
        //}
    }

    void OnDestroy()
    {
        stream?.Close();
        client?.Close();
    }
}
