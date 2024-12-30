using System.Collections.Generic;
using UnityEngine;
using Quaternion = NetData.Quaternion;
using Vector3 = NetData.Vector3;

namespace NetData {
    public struct Vector3
    {
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x;
        public float y;
        public float z;
    }

    public struct Quaternion
    {
        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float x;
        public float y;
        public float z;
        public float w;
    }
}


public static class VecHelp {
    public static Vector3 ToVector3(this UnityEngine.Vector3 v3) {
        Vector3 vector3 = new Vector3();
        vector3.x = v3.x;
        vector3.y = v3.y;
        vector3.z = v3.z;
        return vector3;
    }
    
    public static Quaternion ToQuaternion(this UnityEngine.Quaternion qua) {
        Quaternion quaternion = new Quaternion();
        quaternion.x = qua.x;
        quaternion.y = qua.y;
        quaternion.z = qua.z;
        quaternion.w = qua.w;
        return quaternion;
    }
    
    public static UnityEngine.Quaternion ToQuaternion(this Quaternion qua) {
        UnityEngine.Quaternion quaternion = new UnityEngine.Quaternion();
        quaternion.x = qua.x;
        quaternion.y = qua.y;
        quaternion.z = qua.z;
        quaternion.w = qua.w;
        return quaternion;
    }
    
    public static UnityEngine.Vector3 ToVector3(this Vector3 v3) {
        UnityEngine.Vector3 vector3 = new UnityEngine.Vector3();
        vector3.x = v3.x;
        vector3.y = v3.y;
        vector3.z = v3.z;
        return vector3;
    }
}

