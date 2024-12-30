using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public partial class Game
{
    public static EventSystemManager Event => Get<EventSystemManager>();
    public static UGUIManager UI => Get<UGUIManager>();
    public static ClientNetwork ClientNet => Get<ClientNetwork>();
    public static ServerNetwork ServerNet => Get<ServerNetwork>();
    public static ExcelManager Excel => Get<ExcelManager>();
    public static TimerManager TimerManager => Get<TimerManager>();
    public static AudioManager AudioManager => Get<AudioManager>();
    public static SaveLoadManager Save => Get<SaveLoadManager>();
    public static Camera Camera => Get<CameraManager>().camera;
    public static CameraManager CameraManager => Get<CameraManager>();
    public static LanguageManager Language => Get<LanguageManager>();
}
