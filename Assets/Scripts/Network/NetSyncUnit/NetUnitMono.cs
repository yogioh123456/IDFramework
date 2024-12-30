using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetUnitMono : MonoBehaviour
{
    public TagType type;
    public ushort id;
}

public enum TagType
{
    None,
    Push,
}