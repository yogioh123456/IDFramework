public class Main
{
    public static void Init()
    {
        Game.Init();
        CodeLoader.Instance.Update += () => { Game.Update(); };
        CodeLoader.Instance.FixedUpdate += () => { Game.FixedUpdate(); };
        CodeLoader.Instance.LateUpdate += () => { Game.LateUpdate(); };
        CodeLoader.Instance.OnApplicationQuit += () => { Game.OnApplicationQuit(); };

        Game.Get<MainLogic>().Init();
    }

    public static void Quit() {
        Game.OnApplicationQuit();
    }
}