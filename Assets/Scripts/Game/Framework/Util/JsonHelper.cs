
using UnityEngine;

public static class JsonHelper
{
    public static Vector3Json ToVector3Json(this Vector3 v3)
    {
        Vector3Json vector3Json = new Vector3Json();
        vector3Json.x = v3.x;
        vector3Json.y = v3.y;
        vector3Json.z = v3.z;
        return vector3Json;
    }
    
    public static Vector3 ToVector3(this Vector3Json v3)
    {
        Vector3 vector3 = new Vector3();
        vector3.x = v3.x;
        vector3.y = v3.y;
        vector3.z = v3.z;
        return vector3;
    }
}

public class Vector3Json
{
    public float x;
    public float y;
    public float z;
}
