using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoadHelper
{
    public static AudioClip LoadAudio(string path) 
    {
        return Resources.Load<AudioClip>(path);
    }
    
    public static async Task<byte[]> LoadBytes(string url)
    {
        string path = Path.Combine(Application.streamingAssetsPath, url);
        var getRequest = UnityWebRequest.Get(path);
        await getRequest.SendWebRequest();
        byte[] data = getRequest.downloadHandler.data;
        return data;
    }
}

public static class ExtensionMethods
{
    public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOp.completed += obj => { tcs.SetResult(null); };
        return ((Task)tcs.Task).GetAwaiter();
    }
}