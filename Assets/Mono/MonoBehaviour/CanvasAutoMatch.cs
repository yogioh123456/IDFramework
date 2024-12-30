using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAutoMatch : MonoBehaviour
{
    public CanvasScaler scaler;
    // Start is called before the first frame update
    void Start()
    {
        if (scaler != null)
        {
            Match();
        }
    }

    // Update is called once per frame
    void Match()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        //设计分辨率
        Vector2 v2 = scaler.referenceResolution;
        float s1 = v2.x / v2.y;
        //屏幕分辨率
        float s2 = (float)Screen.width / Screen.height;
        if (s2 >= s1)
        {
            scaler.matchWidthOrHeight = 1;
        }
        else
        {
            scaler.matchWidthOrHeight = 0;
        }
    }
}
