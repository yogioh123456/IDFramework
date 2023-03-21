using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class HotReloadTool
{
    static HotReloadTool()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("热重载", EditorGUIUtility.FindTexture("PlayButton"))))
        {
            Debug.Log("热重载");
            BuildAssemblieEditor.BuildLogic();
        }
    }
}