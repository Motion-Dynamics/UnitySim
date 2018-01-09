using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerListener : MonoBehaviour {

    byte[] bytes = new byte[1024];
    Socket client;
    String headers;

    float fx, fy, fz;

    // Use this for initialization
    void Start () {
        try
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("kerlin.tech");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            print(ipAddress);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5050);

            client = new Socket(ipAddress.AddressFamily,
                 SocketType.Stream, ProtocolType.Tcp);
            client.Connect(remoteEP);

            byte[] msg = Encoding.ASCII.GetBytes("listener");

            int bytesSent = client.Send(msg);

            int bytesRec = client.Receive(bytes);
            headers = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            print("headers: " + headers);

            msg = Encoding.ASCII.GetBytes("ok");

            bytesSent = client.Send(msg);

            bytesRec = client.Receive(bytes);
            data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            print("data: " + data);
            String[] values = data.Split(',');
            fx = float.Parse(values[7]);
            fy = float.Parse(values[6]);
            fz = float.Parse(values[8]);


        } catch(Exception e)
        {
            print(e);
        }

    }
    String data;
	// Update is called once per frame
	void Update () {
        try
        {
            byte[] msg = Encoding.ASCII.GetBytes("ok");

            int bytesSent = client.Send(msg);

            int bytesRec = client.Receive(bytes);
            data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            print("data: " + data);
            String[] values = data.Split(',');
            float x = (float)(float.Parse(values[7]));
            float y = (float)(float.Parse(values[6]));
            float z = (float)(float.Parse(values[8]));
            transform.rotation = Quaternion.Euler((x-fx), (y-fy), (z-fz));
        } catch(Exception e)
        {
            print(e);
        }
    }
}
