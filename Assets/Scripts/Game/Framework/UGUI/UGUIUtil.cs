using UnityEngine.Events;
using UnityEngine.UI;

public static class UGUIUtil {
    //UI按钮统一添加事件，添加统一效果
    public static void AddButtonEvent(this Button button, UnityAction ac)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            ac();
            if (button.GetComponent<ButtonAudio>() != null)
            {
                button.GetComponent<ButtonAudio>().PlayButtonAudio();
            }
        });
    }
}
