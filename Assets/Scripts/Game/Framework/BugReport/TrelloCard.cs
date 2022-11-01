using System;

/// <summary>
/// Trello 卡片
/// </summary>
[Serializable]
public class TrelloCard
{
    public string name;
    public string description;
    public string position = "bottom";
    public string listID = string.Empty;
}
