using System;

public class CodeLoader
{
    public static CodeLoader Instance = new CodeLoader();
    public Action Update;
    public Action FixedUpdate;
    public Action LateUpdate;
    public Action OnApplicationQuit;
}
