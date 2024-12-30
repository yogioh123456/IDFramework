using System;
using System.Collections.Generic;
using RiptideNetworking;
using UnityEngine;

public class BuffManagerServer : IUpdate {
    
    private List<NetSyncUnitServer> buffList = new List<NetSyncUnitServer>();
    private int buffId;

    public BuffManagerServer()
    {
        //AddBuff<LevelServer>("level", 0, false, new byte[] {1});
    }

    public NetSyncUnitServer GetBuff(string buffName)
    {
        foreach (var one in buffList)
        {
            if (buffName.Equals(one.unitName))
            {
                return one;
            }
        }
        return null;
    }
    
    public NetSyncUnitServer AddBuff<T>(ushort playerId, bool sendToClient, byte[] data) where T : NetSyncUnitServer, new() {
        string input = typeof(T).Name;
        string keyword = "Server";
        int index = input.IndexOf(keyword, StringComparison.Ordinal);
        string result = index > 0 ? input.Substring(0, index) : input;
        
        T buff = new T();
        buff.unitId = buffId;
        buff.unitName = result;
        buff.myPlayerId = playerId;
        buff.data = data;
        Debug.Log("服务器buff" + result + playerId);
        buff.Init();
        buffList.Add(buff);
        buffId++;
        
        //通知客户端创建buff
        if (sendToClient)
        {
            NetBuffData netBuffData = new NetBuffData();
            netBuffData.unitId = buff.unitId;
            netBuffData.unitName = buff.unitName;
            netBuffData.myPlayerId = buff.myPlayerId;
            netBuffData.position = buff.position;
            netBuffData.rotation = buff.rotation;
            netBuffData.data = buff.data;
            Game.ServerNet.SendToAll(NetMsg.MessageId.CreateNetUnit, netBuffData.DataToBytes());
        }

        return buff;
    }
    
    
    [MessageHandler((ushort)NetMsg.MessageId.CreateNetUnit)]
    private static void CreateNetUnit(ushort fromClientId, Message message)
    {
        //ushort playerId = message.GetUShort();
        string buffName = message.GetString();
        int id = message.GetInt();
        Debug.Log($"创建同步物体:  {buffName}    " + fromClientId);

        if (buffName.Equals("Player"))
        {
            //主机玩家
            if (fromClientId == 1)
            {
                //Game.Get<BuffManagerServer>().AddBuff<LevelServer>("level", 0, false, new byte[] {1});
            }
            
            NetRoleData netRoleData = new NetRoleData();
            netRoleData.isMonster = false;
            netRoleData.playerId = fromClientId;
            netRoleData.id = id;
        }
    }
    
    [MessageHandler((ushort)NetMsg.MessageId.CheckAllBuff)]
    private static void CheckAllBuff(ushort fromClientId, Message message)
    {
        Game.Get<BuffManagerServer>().SendAllBuffData(fromClientId);
    }
    
    [MessageHandler((ushort)NetMsg.MessageId.SendBuffData)]
    private static void SendBuffData(ushort fromClientId, Message message)
    {
        int buffId = message.GetInt();
        int type = message.GetInt();
        //Debug.Log("xxx" + xxx);
        //return;
        
        byte[] datas = message.GetBytes(true);
        Game.Get<BuffManagerServer>().SendBuffData(fromClientId, buffId, type, datas);
    }

    [MessageHandler((ushort)NetMsg.MessageId.SyncBuffData)]
    private static void SyncBuffData(ushort fromClientId, Message message)
    {
        int buffId = message.GetInt();
        int type = message.GetInt();
        //Debug.Log("xxx" + xxx);
        //return;
        
        byte[] datas = message.GetBytes(true);
        Game.Get<BuffManagerServer>().SyncBuffData(buffId, type, datas);
    }
    
    public void Update()
    {
        foreach (var one in buffList)
        {
            one.Update();
        }
    }

    public void SendBuffData(ushort fromClientId, int buffId, int type, byte[] data)
    {
        foreach (var buff in buffList)
        {
            if (buff.unitId == buffId)
            {
                buff.FromClientData(fromClientId, type, data);
                break;
            }
        }
    }

    private void SyncBuffData(int buffId, int type, byte[] data)
    {
        foreach (var buff in buffList)
        {
            if (buff.unitId == buffId)
            {
                buff.SyncClientData(type, data);
                break;
            }
        }
    }
    
    private void SendAllBuffData(ushort id)
    {
        foreach (var buff in buffList) {
            NetBuffData netBuffData = new NetBuffData();
            netBuffData.unitId = buff.unitId;
            netBuffData.unitName = buff.unitName;
            netBuffData.myPlayerId = buff.myPlayerId;
            netBuffData.position = buff.position;
            netBuffData.rotation = buff.rotation;
            netBuffData.data = buff.data;
            Game.ServerNet.Send(id, NetMsg.MessageId.CreateNetUnit, netBuffData.DataToBytes());
        }
    }
}
