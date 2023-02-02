using System;
using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

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
}
