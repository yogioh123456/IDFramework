using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    [Header("可选,无则播放默认音频")]
    [Tooltip("可选,无则播放默认音频")]
    public AudioClip clip;

    public void PlayButtonAudio()
    {
        if (clip == null)
        {
            //AudioManager.Instance.PlayMusic(Resources.Load<AudioClip>("click"));
        }
        else
        {
            //AudioManager.Instance.PlayMusic(clip);
        }
    }

}
