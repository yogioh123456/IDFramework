using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频管理器
/// </summary>
public class AudioManager : Entity, IUpdate
{
    private GameObject gameObject;
    private List<AudioSource> audioSourcePool = new List<AudioSource>(16);

    public AudioManager()
    {
        gameObject = new GameObject("AudioManager");
        if (ES3.KeyExists("musicVol"))
        {
            MusicVol = ES3.Load<float>("musicVol");
        }

        if (ES3.KeyExists("bgmVol"))
        {
            bgmVol = ES3.Load<float>("bgmVol");
        }
    }

    /// <summary>
    /// 音乐大小
    /// </summary>
    private float musicVol = 0.5f;

    public float MusicVol
    {
        get { return musicVol; }
        set
        {
            musicVol = value;
            for (int i = 0; i < soundList.Count; i++)
            {
                Sound s = soundList[i];
                s.source.volume = value;
            }
        }
    }

    /// <summary>
    /// BGM大小
    /// </summary>
    public float bgmVol
    {
        get { return bgmSource.volume; }
        set { bgmSource.volume = value; }
    }

    /// <summary>
    /// BGM是否静音
    /// </summary>
    public bool bgmIsMute
    {
        get { return bgmSource.mute; }
        set { bgmSource.mute = value; }
    }

    /// <summary>
    /// 音效是否静音
    /// </summary>
    public bool musicIsMute = false;

    /// <summary>
    /// 播放中的音频字典
    /// </summary>
    public List<Sound> soundList = new List<Sound>();
    private Dictionary<string, Sound> soundDic = new Dictionary<string, Sound>();

    public Sound PlayAudio(AudioClip clip, bool isLoop = false) {
        AudioSource source = GetAudioSource();
        source.mute = musicIsMute;
        source.loop = isLoop;
        source.volume = MusicVol;
        Sound sound = new Sound(this, clip, source, "");
        soundList.Add(sound);
        return sound;
    }

    public void PlayAudioOne(string musicPath)
    {
        if (string.IsNullOrEmpty(musicPath))
        {
            return;
        }

        if (!soundDic.ContainsKey(musicPath))
        {
            soundDic.Add(musicPath, PlayAudio(musicPath));
        }
        soundDic[musicPath].Play();
    }
    
    public Sound PlayAudio(string musicPath)
    {
        if (string.IsNullOrEmpty(musicPath))
        {
            return null;
        }

        AudioSource source = GetAudioSource();
        source.mute = musicIsMute;
        source.volume = MusicVol;
        AudioClip ac = LoadAudio(musicPath);
        Sound sound = new Sound(this, ac, source, musicPath);
        soundList.Add(sound);
        return sound;
    }

    public void Update()
    {
        for (int i = soundList.Count - 1; i >= 0; i--)
        {
            Sound _sound = soundList[i];
            _sound.Update();
            if (_sound.source == null)
            {
                soundList.RemoveAt(i);
                if (!string.IsNullOrEmpty(_sound.path) && soundDic.ContainsKey(_sound.path))
                {
                    soundDic.Remove(_sound.path);
                }
            }
        }
    }

    /// <summary>
    /// 背景音乐(只能存在一个，并且是循环的，播放新的会自动替换旧的)
    /// </summary>
    private AudioSource bgmSource;

    private string bgmName = "";

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="isLoop"></param>
    public void PlayBGM(AudioClip clip, bool isLoop)
    {
        bgmSource.clip = clip;
        bgmSource.loop = isLoop;
        bgmSource.Play();
    }

    public void PlayBGM(string musicPath, bool isLoop)
    {
        AudioClip clip = LoadAudio(musicPath);
        if (bgmName.Equals(musicPath))
        {
            return;
        }

        if (clip == null)
        {
            Debug.LogError("音乐路径无法加载");
        }

        bgmSource.clip = clip;
        bgmSource.loop = isLoop;
        bgmSource.Play();
        bgmName = musicPath;
    }

    /// <summary>
    /// 暂停/恢复背景音乐
    /// </summary>
    public void PauseBGM()
    {
        if (bgmSource.clip == null)
        {
            Debug.LogWarning("没有设置背景音乐");
            return;
        }

        if (bgmSource.isPlaying)
        {
            bgmSource.Pause();
        }
        else
        {
            bgmSource.UnPause();
        }
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    private AudioClip LoadAudio(string path)
    {
        return Resources.Load<AudioClip>(path);
    }

    private AudioSource GetAudioSource() {
        AudioSource audioSource;
        if (audioSourcePool.Count == 0) {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        } else {
            audioSource = audioSourcePool[0];
            audioSourcePool.Remove(audioSource);
        }
        
        audioSource.enabled = true;
        return audioSource;
    }
    
    public void ReleaseAudioSource(AudioSource audio) {
        audio.enabled = false;
        audioSourcePool.Add(audio);
    }
}