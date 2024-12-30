using System;
using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

//如果需要Host模式，即客户端服务器都跑在一个主机上，可启用此脚本

public class ServerNetwork : IFixedUpdate, IApplicationQuit {
    private Server server;
    private delegate void ServerMessageReceived(object sender, ServerMessageReceivedEventArgs e);

    //private ServerSyncManager serverSyncManager;

    private uint timeTick;
    
    public ServerNetwork() {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError,false);
        server = new Server();
        server.MessageReceived += OnMessageReceived;
        server.ClientDisconnected += OnClientDisconnected;
        server.ClientConnected += ClientConnected;
        Debug.Log("==服务器网络启动==");
        
        //server.CreateMessageHandlersDictionary
        //StartServer(7778, 100);
    }

    private void ClientConnected(object data, ServerClientConnectedEventArgs e)
    {
        Game.Get<ServerSyncManager>().ClientConnected(e.Client.Id);
    }
    
    private void OnMessageReceived(object sender, ServerMessageReceivedEventArgs e) {
        Game.Get<ServerSyncManager>().AddCmd(e, timeTick, sender);
    }

    private void OnClientDisconnected(object data, ClientDisconnectedEventArgs e) {
        Game.Event.Dispatch("ClientDisconnected", e.Id);
    }
    
    public void StartServer(ushort port, ushort maxNum) {
        server.Start(port, maxNum);
        Game.AddComp<NetSyncUnitServer>();
    }

    public void FixedUpdate() {
        if (server.IsRunning) {
            server.Tick();
            timeTick += 1;
        }
    }

    public void OnApplicationQuit() {
        server.Stop();
    }

    public void SendToAll(Message message, bool shouldRelease = true) {
        server.SendToAll(message);
    }
    
    public void Send(Message message, ushort clientId,bool shouldRelease = true) {
        server.Send(message, clientId, shouldRelease);
    }
    
    public void Send(ushort clientId, Enum id)
    {
        //消息发送
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        server.Send(message, clientId);
    }
    
    public void Send<T>(ushort clientId, Enum id, T t)
    {
        //消息发送
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        MessageAdd(message, t);
        server.Send(message, clientId);
    }
    
    public void Send<T,K>(ushort clientId, Enum id, T t, K k)
    {
        //消息发送
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        MessageAdd(message, t);
        MessageAdd(message, k);
        server.Send(message, clientId);
    }
    
    public void Send<T,K,V>(ushort clientId, Enum id, T t, K k, V v)
    {
        //消息发送
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        MessageAdd(message, t);
        MessageAdd(message, k);
        MessageAdd(message, v);
        server.Send(message, clientId);
    }
    
    public void Send<T,K,V,X>(ushort clientId, Enum id, T t, K k, V v, X x)
    {
        //消息发送
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        MessageAdd(message, t);
        MessageAdd(message, k);
        MessageAdd(message, v);
        MessageAdd(message, x);
        server.Send(message, clientId);
    }

    public void SendToAll(Enum id)
    {
        //消息发送
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        server.SendToAll(message);
    }

    public void SendToAll<T>(Enum id, T t)
    {
        //消息发送
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        MessageAdd(message, t);
        server.SendToAll(message);
    }

    public void SendToAll<T, K>(Enum id, T t, K k)
    {
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        MessageAdd(message, t);
        MessageAdd(message, k);
        server.SendToAll(message);
    }

    public void SendToAll<T, K, V>(Enum id, T t, K k, V v)
    {
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        MessageAdd(message, t);
        MessageAdd(message, k);
        MessageAdd(message, v);
        server.SendToAll(message);
    }
    
    public void SendToAll<T, K, V, X>(Enum id, T t, K k, V v, X x)
    {
        Message message = Message.Create(MessageSendMode.reliable, id, shouldAutoRelay: true);
        MessageAdd(message, t);
        MessageAdd(message, k);
        MessageAdd(message, v);
        MessageAdd(message, x);
        server.SendToAll(message);
    }
    
    private delegate void MessageDelegate<Message, T>(Message msg, T t);
    private void MessageAdd<T>(Message message, T t) {
        if (typeof(T) == typeof(int)) {
            MessageDelegate<Message, int> add = AddInt;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(float)) {
            MessageDelegate<Message, float> add = AddFloat;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(bool)) {
            MessageDelegate<Message, bool> add = AddBool;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(string)) {
            MessageDelegate<Message, string> add = AddString;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(double)) {
            MessageDelegate<Message, double> add = AddDouble;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(long)) {
            MessageDelegate<Message, long> add = AddLong;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(short)) {
            MessageDelegate<Message, short> add = AddShort;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(ushort)) {
            MessageDelegate<Message, ushort> add = AddUShort;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(byte)) {
            MessageDelegate<Message, byte> add = AddByte;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(Quaternion)) {
            MessageDelegate<Message, Quaternion> add = AddQuaternion;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(Vector3)) {
            MessageDelegate<Message, Vector3> add = AddVector3;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(Vector2)) {
            MessageDelegate<Message, Vector2> add = AddVector2;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        } else if (typeof(T) == typeof(byte[])) {
            MessageDelegate<Message, byte[]> add = AddBytes;
            (add as MessageDelegate<Message, T>)?.Invoke(message, t);
        }
    }
    
    #region AddMsg
    private void AddShort(Message message, short data) {
        message.AddShort(data);
    }
    private void AddUShort(Message message, ushort data) {
        message.AddUShort(data);
    }
    private void AddLong(Message message, long data) {
        message.AddLong(data);
    }
    private void AddByte(Message message, byte data) {
        message.AddByte(data);
    }
    private void AddBytes(Message message, byte[] data) {
        message.AddBytes(data, true, true);
    }
    private void AddBool(Message message, bool data) {
        message.AddBool(data);
    }
    private void AddDouble(Message message, double data) {
        message.AddDouble(data);
    }
    private void AddInt(Message message, int data) {
        message.AddInt(data);
    }
    private void AddFloat(Message message, float data) {
        message.AddFloat(data);
    }
    private void AddString(Message message, string data) {
        message.AddString(data);
    }
    private void AddQuaternion(Message message, Quaternion quaternion) {
        message.AddQuaternion(quaternion);
    }
    private void AddVector3(Message message, Vector3 vector3) {
        message.AddVector3(vector3);
    }
    private void AddVector2(Message message, Vector2 vector2) {
        message.AddVector2(vector2);
    }
    #endregion
}
