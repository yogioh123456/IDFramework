using System;
using System.Text;
using UNetwork;
using UnityEngine;

public class TestClient : MonoBehaviour
{
    public string ip = "127.0.0.1";
    public int port = 12346;

//    private string address = "10.200.10.192:3655";
    public string sendMessage = "client";
    public static long starttime = 0;
    public bool isStart = false;

    public void MyStart() {
        KCPClientManager client = Game.Get<KCPClientManager>();
        client.Init();
        client.InitService(NetworkProtocol.TCP);
        client.MessagePacker = new ProtobufPacker();
        client.MessageDispatcher = new OuterMessageDispatcher();

        client.Connect(ip,port);
        client.OnConnect += OnConnect;
        client.OnError += OnError;
        client.OnMessage += OnMessage;
    }

    private void OnMessage(byte[] obj)
    {
        var msg = Encoding.UTF8.GetString(obj);
        Debug.Log($"Receive{obj.Length}" + msg);
    }

    private void OnError(int e)
    {
        Debug.LogError("网络错误：" + e);
    }

    private void OnConnect(int c)
    {
        Debug.Log("连接成功");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) {
            MyStart();
            isStart = true;
        }
        if (Input.GetMouseButtonDown(0) &&　isStart)
        {
            var data = Encoding.UTF8.GetBytes(sendMessage);
            Debug.Log($"Send{data.Length}:" + sendMessage);

            Game.Get<KCPClientManager>().Send(data);
            starttime = GetTimeStamp();
        }
    }

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    public static long GetTimeStamp()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }

    public static void Receive()
    {
        var inteval = GetTimeStamp() - starttime;
        Debug.LogWarning(inteval);
    }
}