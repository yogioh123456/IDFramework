using System;
using System.Collections.Generic;
using System.Reflection;

public partial class Game
{
    public static EventSystemManager Event => Get<EventSystemManager>();
    public static UGUIManager UI => Get<UGUIManager>();
    public static ClientNetwork ClientNet => Get<ClientNetwork>();
    public static ExcelManager Excel => Get<ExcelManager>();
    public static TimerManager Time => Get<TimerManager>();
    public static AudioManager AudioManager => Get<AudioManager>();
    public static SaveLoadManager SaveLoad => Get<SaveLoadManager>();
}
