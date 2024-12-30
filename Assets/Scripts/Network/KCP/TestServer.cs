using System;
using System.Text;
using UNetwork;
using UnityEngine;

public class TestServer : MonoBehaviour
{
    public string ip = "127.0.0.1";
    public int port = 12346;
    
    public string sendMessage = "server";
    public static long starttime = 0;

    public void MyStart() {
        Debug.Log("启动服务器");
        KCPServerManager server = Game.Get<KCPServerManager>();
        server.Init();
        server.InitService(NetworkProtocol.TCP);
        server.MessagePacker = new ProtobufPacker();
        server.MessageDispatcher = new OuterMessageDispatcher();

        server.Connect(ip, port);
        server.OnConnect += OnConnect;
        server.OnError += OnError;
        server.OnMessage += OnMessage;
    }

    private void OnMessage(byte[] obj)
    {
        var msg = Encoding.UTF8.GetString(obj);
        Debug.Log(msg);
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
        if (Input.GetKeyDown(KeyCode.K)) {
            MyStart();
        }
        if (Input.GetMouseButtonDown(0))
        {
            return;
            var data = Encoding.UTF8.GetBytes(sendMessage);
            Debug.Log($"Send{data.Length}:" + sendMessage);

            Game.Get<KCPServerManager>().Send(data);
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