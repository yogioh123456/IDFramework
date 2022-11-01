using UnityEngine;

public enum GameMode
{
    Develop,
    Release
}

public class Launch : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;

    public static GameMode GameMode;
    public static AssetBundle assetBundle;

    void Start()
    {
#if !UNITY_EDITOR
        gameMode = GameMode.Release;
#endif
        GameMode = gameMode;
        AssetManager.Init();

        if (GameMode == GameMode.Develop)
        {
#if UNITY_EDITOR
            //AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "HotFix");
#endif
        }
        else
        {
            string path = $"{Application.streamingAssetsPath}/common";
            assetBundle = AssetBundle.LoadFromFile(path);
            TextAsset dllBytes1 = assetBundle.LoadAsset<TextAsset>("HotFix.dll.bytes");
            System.Reflection.Assembly.Load(dllBytes1.bytes);
        }

        Game.Init();
        if (GameMode == GameMode.Develop) {
            AssetManager.LoadPrefab("Prefabs/Main");
        } else {
            //TODO:此处读取的是AB包中的Prefab，还需要优化
            GameObject testPrefab = Instantiate(assetBundle.LoadAsset<GameObject>("Main.prefab"));
        }
    }
}