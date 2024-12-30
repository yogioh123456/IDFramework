using RiptideNetworking;

public class ServerMainLogic {
    public ServerMainLogic() {
        Game.AddComp<BuffManagerServer>();
    }
    
        
    [MessageHandler((ushort)NetMsg.MessageId.Broadcast)]
    private static void Broadcast(ushort fromClientId, Message message)
    {
        string str = message.GetString();
        var data = message.GetBytes();
        Game.ServerNet.SendToAll(NetMsg.MessageId.Broadcast, str, data);
    }
}
