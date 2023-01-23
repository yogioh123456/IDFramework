using UnityEditor;

public class DevToolEditor {
    [MenuItem("Tools/暂停游戏 _F5")]
    static void PauseGame() {
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }
}
