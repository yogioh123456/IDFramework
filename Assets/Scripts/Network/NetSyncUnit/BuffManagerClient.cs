using System;
using System.Collections.Generic;
using RiptideNetworking;
using UnityEngine;

public class BuffManagerClient : IUpdate, IFixedUpdate
{
    private List<NetSyncUnitClient> buffList = new List<NetSyncUnitClient>();

    public BuffManagerClient()
    {
        
    }
    
    public T GetBuff<T>(string buffName) where T: NetSyncUnitClient
    {
        foreach (var one in buffList)
        {
            if (buffName.Equals(one.unitName))
            {
                return (T)one;
            }
        }
        return null;
    }

    public void AddClientBuff(string className, NetBuffData netBuffData)
    {
        Type type = Type.GetType(className);
        NetSyncUnitClient buff = (NetSyncUnitClient)Activator.CreateInstance(type);
        buff.unitId = netBuffData.unitId;
        buff.unitName = netBuffData.unitName;
        buff.myPlayerId = netBuffData.myPlayerId;
        buff.position = netBuffData.position;
        buff.rotation = netBuffData.rotation;
        buff.Init(netBuffData.data);
        buffList.Add(buff);
        Debug.Log($"创建客户端Buff{netBuffData.unitName}");
    }
    
    public void AddClientBuff<T>(int unitId, string unitName, ushort playerId, byte[] data) where T : NetSyncUnitClient, new() 
    {
        T buff = new T();
        buff.unitId = unitId;
        buff.unitName = unitName;
        buff.myPlayerId = playerId;
        buff.Init(data);
        buffList.Add(buff);
        Debug.Log($"创建客户端Buff{unitName}");
    }
    
    [MessageHandler((ushort)NetMsg.MessageId.CreateNetUnit)]
    private static void CreateNetUnitClient(Message message) {
        var x = message.GetBytes();
        NetBuffData netBuffData = x.BytesToData<NetBuffData>();
        var unitName = netBuffData.unitName;
        Debug.Log("buff  client  " + unitName);
        Game.Get<BuffManagerClient>().AddClientBuff($"{unitName}Client", netBuffData);
    }

    public void Update()
    {
        foreach (var one in buffList)
        {
            one.Update();
        }
    }
    
    [MessageHandler((ushort)NetMsg.MessageId.ClearBuff)]
    private static void ClearBuffData(Message message)
    {
        int buffId = message.GetInt();
        Game.Get<BuffManagerClient>().ClearBuff(buffId);
    }

    [MessageHandler((ushort)NetMsg.MessageId.SendBuffData)]
    private static void SendBuffData(Message message)
    {
        int buffId = message.GetInt();
        int key = message.GetInt();
        byte[] datas = message.GetBytes(true);
        Game.Get<BuffManagerClient>().SendToBuffData(buffId, key, datas);
    }
    
    [MessageHandler((ushort)NetMsg.MessageId.SyncBuffData)]
    private static void SyncBuffData(Message message)
    {
        int buffId = message.GetInt();
        int key = message.GetInt();
        byte[] datas = message.GetBytes(true);

        //Debug.Log($"buffid{buffId}  key{key}");

        Game.Get<BuffManagerClient>().SendToBuffData(buffId, key, datas);
    }
    
    public void SendToBuffData(int buffId, int key, byte[] data)
    {
        foreach (var buff in buffList)
        {
            if (buff.unitId == buffId)
            {
                buff.ServerData(key, data);
                break;
            }
        }
    }

    public void ClearBuff(int buffId)
    {
        NetSyncUnitClient removeBuff = null;
        foreach (var buff in buffList)
        {
            if (buff.unitId == buffId)
            {
                buff.Clear();
                removeBuff = buff;
                break;
            }
        }

        if (removeBuff != null)
        {
            buffList.Remove(removeBuff);
        }
    }
    
    public void FixedUpdate()
    {
        foreach (var one in buffList)
        {
            one.FixedUpdate();
        }
    }
}
