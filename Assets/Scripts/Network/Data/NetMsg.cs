public class NetMsg {
    public enum MessageId : ushort
    {
        Chat = 1,
        Register,
        Login,
        LoginSuccess,
        LoginInfo,
        PlayerData,
        CollectSpringTick,
        startTest,
        testMessage,

        CreateRoom,
        CreatePrefab,
        DisposePrefab,
        Move,
        PlayAnim,
        
        CreateNetUnit,
        SendBuffData,
        CheckAllBuff,
        SyncBuffData,
        PushObject,
        SceneMessage,
        Broadcast,//通用广播消息，客户端发到服务器，服务器再广播给所有客户端；第一个参数是字符串，第二个参数是byte[]
        ClearBuff,
        ResetLevel,
    }
}
