using System.Text;
using UNetwork;
using UnityEngine;

public class KCPServerLogic {
    
    public string ip = "127.0.0.1";
    public int port = 12346;
    private bool isStart;
    
    public void Init() {
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
        isStart = true;
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
    
    public void OnDispose() {
        if (isStart) {
            KCPServerManager server = Game.Get<KCPServerManager>();
            server.OnConnect -= OnConnect;
            server.OnError -= OnError;
            server.OnMessage -= OnMessage;
            isStart = false;
        }
    }
}
