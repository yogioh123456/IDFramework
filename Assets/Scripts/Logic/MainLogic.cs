using RiptideNetworking;
using UNetwork;
using UnityEngine;

public class MainLogic {
    
    /// <summary>
    /// 游戏主逻辑入口
    /// </summary>
    public void Init()
    {
        Game.AddComp<KCPClientManager>();
        Game.AddComp<KCPClientLogic>();
        Debug.Log("框架成功运行");
        Game.UI.OpenUIPanel<UI_KCP>();
        //Game.UI.OpenUIPanel<UI_Start>();
        //Game.ClientNet.connectedAction = GameStart;
        
    }

    private void GameStart() {
        Debug.Log("游戏开始");
        

        //发送登录成功
        int id = 1;
        Game.ClientNet.Send(NetMsg.MessageId.CheckAllBuff);
        
        Game.ClientNet.Send(NetMsg.MessageId.CreateNetUnit, "Player", 0);
        
        Game.UI.BackUIPanel();
        Game.UI.OpenUI<UI_DevTool>("init");

    }
    
    [MessageHandler((ushort)NetMsg.MessageId.Broadcast)]
    private static void Broadcast(Message message)
    {
        string str = message.GetString();
        byte[] data = message.GetBytes();
        Game.Event.Dispatch(str, data);
    }
}
