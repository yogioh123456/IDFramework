using System;
using System.Collections;
using System.Collections.Generic;
using RiptideNetworking;
using UnityEngine;

//继承此类的必须以Server结尾
public class NetSyncUnitServer 
{
    public int unitId;
    public string unitName;
    public ushort myPlayerId;
    public Vector3 position;
    public Quaternion rotation;
    public byte[] data;
    protected Dictionary<int, Action<ushort, byte[]>> dic = new Dictionary<int, Action<ushort, byte[]>>(8);

    public void Init()
    {
        OnInit();
    }
    
    public virtual void OnInit()
    {
        
    }

    public virtual void Update()
    {
        
    }
    
    public virtual void Clear()
    {
        
    }

    public void RemoveSelf()
    {
        Clear();
        Game.ServerNet.SendToAll(NetMsg.MessageId.ClearBuff, unitId);
    }
    
    public void SetSyncData<T>(int key, T value)
    {
        //同步数据到客户端
        var data = value.DataToBytes();
        Game.ServerNet.SendToAll(NetMsg.MessageId.SendBuffData, unitId, key, data);
    }

    public void FromClientData(ushort fromClientId, int type, byte[] data)
    {
        dic[type].Invoke(fromClientId, data);
    }
    
    public void SyncClientData(int type, byte[] data)
    {
        Game.ServerNet.SendToAll(NetMsg.MessageId.SyncBuffData, unitId, type, data);
    }
}
