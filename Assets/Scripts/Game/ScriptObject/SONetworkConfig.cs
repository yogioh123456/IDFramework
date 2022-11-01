using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SONetworkConfig")]
public class SONetworkConfig : ScriptableObject {
    public string ip;
    public ushort port;
}
