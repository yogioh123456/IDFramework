using System.Text;
using UNetwork;
using UnityEngine;

public class KCPClientLogic : IApplicationQuit {

    public string ip = "127.0.0.1";
    public int port = 12346;
    private bool isStart;
    
    public void Init() {
        KCPClientManager client = Game.Get<KCPClientManager>();
        client.Init();
        client.InitService(NetworkProtocol.TCP);
        client.MessagePacker = new ProtobufPacker();
        client.MessageDispatcher = new OuterMessageDispatcher();

        client.Connect(ip,port);
        client.OnConnect += OnConnect;
        client.OnError += OnError;
        client.OnMessage += OnMessage;
        isStart = true;
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

    public void OnApplicationQuit() {
        if (isStart) {
            KCPClientManager client = Game.Get<KCPClientManager>();
            client.OnConnect -= OnConnect;
            client.OnError -= OnError;
            client.OnMessage -= OnMessage;
            isStart = false;
        }
    }
}
