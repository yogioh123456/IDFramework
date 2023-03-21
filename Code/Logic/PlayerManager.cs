using UnityEngine;

public class PlayerManager: IUpdate, IApplicationQuit 
{
    private PlayerControl playerControl;
    
    public PlayerManager() {
        playerControl = new PlayerControl();
    }
    
    public void OnApplicationQuit()
    {
        playerControl.Destory();
    }

    public void Update() {
        playerControl.Update();
    }
}


