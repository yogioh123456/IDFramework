using System;
using MemoryPack;
using UnityEngine;

//网络序列化的类必须是 partial 
[MemoryPackable]
public partial class NetBuffData {
    public int unitId;
    public string unitName;
    public ushort myPlayerId;
    public Vector3 position;
    public Quaternion rotation;
    public byte[] data;
}

[MemoryPackable]
public partial class NetRoleData {
    public int unitId;//怪物唯一id
    public int type;//怪物类型
    public int hp;
    public ushort playerId;//代理玩家
    public bool isMonster;
    public int id;//表格配置的id
}

[MemoryPackable]
public partial class NetCreateSkillData {
    public int skillEnum;
    public int attackRoleId;
    public int targetRoleId;
    public Vector3 pos;
}

[MemoryPackable]
public partial class NetPlayerEffect {
    public string effectName;
    public float effectDuration;
    public Vector3 pos;
}