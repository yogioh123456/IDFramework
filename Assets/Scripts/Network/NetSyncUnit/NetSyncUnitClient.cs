using System;
using System.Collections.Generic;
using UnityEngine;

//继承此类的必须以Client结尾
public class NetSyncUnitClient
{
    public int unitId;
    public string unitName;
    public ushort myPlayerId;
    public Vector3 position;
    public Quaternion rotation = Quaternion.identity;
    protected Dictionary<int, Action<byte[]>> netEventDic = new Dictionary<int, Action<byte[]>>(8);
    private Dictionary<int, object> lastSentData = new Dictionary<int, object>(8);

    public void Init(byte[] data)
    {
        netEventDic.Clear();
        OnInit(data);
    }

    protected virtual void OnInit(byte[] data)
    {
        
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }
    
    public virtual void Clear()
    {
        
    }

    public bool IsLocalPlayerBuff => Game.ClientNet.ID == myPlayerId;

    //数据同步给其他客户端，服务器自行处理
    protected void SendDataToServer<T>(int type, T data, bool isRepeat = false)
    {
        if (isRepeat)
        {
            if (lastSentData.ContainsKey(type) && lastSentData[type].Equals(data))
            {
                return;
            }
            lastSentData[type] = data;
        }

        Game.ClientNet.Send(NetMsg.MessageId.SendBuffData, unitId, type, data.DataToBytes());
    }
    
    //数据同步给其他客户端，服务器只负责转发，不存
    protected void SendDataToSync<T>(int type, T data, bool isRepeat)
    {
        if (isRepeat)
        {
            if (lastSentData.ContainsKey(type) && lastSentData[type].Equals(data))
            {
                return;
            }
            lastSentData[type] = data;
        }

        Game.ClientNet.Send(NetMsg.MessageId.SyncBuffData, unitId, type, data.DataToBytes());
    }

    public void ServerData(int key, byte[] data)
    {
        netEventDic[key].Invoke(data);
    }
}
