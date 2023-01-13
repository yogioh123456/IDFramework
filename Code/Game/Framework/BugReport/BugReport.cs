using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BugReport : MonoBehaviour
{
    private Trello trello;
    private const string TRELLO_USER_KEY = "cd4fa6bbc0a4e04154163d6e3dbbb6a0";
    private const string TRELLO_USER_TOKEN = "58c3ba927bd006c7439bc3dc0400711d6b6e0e2af55be352ea7fe5c8461bc09d";
    private static string TRELLO_USER_TOKEN_BOARD => $"不可名状的地牢v{Application.version}";
    private List<string> userListName;
    private Texture2D screenshot;
    private string cardTitle;
    private string cardDescription;
    private string cardList;
    private string condition;
    private string stacktrace;
    private LogType type = LogType.Error;
    public static bool isReporting;
    public bool test;
    public bool close;

    /// <summary>
    /// [Async] 初始化
    /// </summary>
    public async void Start()
    {
        if (close)
        {
            return;
        }
#if UNITY_EDITOR
        if (!test)
        {
            return;
        }
#endif
        if (trello == null)
        {
            trello = new Trello(TRELLO_USER_KEY, TRELLO_USER_TOKEN);

            var pair = await trello.WebRequest_GetUserAllBoards();
            if (pair.Key)
            {
                trello.SetCurrentBoard(TRELLO_USER_TOKEN_BOARD);
                pair = await trello.WebRequest_GetUserAllLists();
                if (pair.Key)
                {
                    SyncList();
                    pair = await CreateNewList();
                    if (pair.Key)
                    {
                        pair = await trello.WebRequest_GetUserAllLists();
                        if (pair.Key)
                        {
                            //初始化成功!
                            RegisterLogCollect();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 同步看板列表
    /// </summary>
    private void SyncList()
    {
        if (userListName == null || userListName.Count <= 0)
        {
            userListName = new List<string>();
            foreach (var listName in trello.cachedUserLists.Keys)
            {
                userListName.Add(listName);
            }
        }
        else
        {
            foreach (var listName in trello.cachedUserLists.Keys)
            {
                if (userListName.Contains(listName) == false)
                {
                    userListName.Add(listName);
                }
            }
        }
    }

    /// <summary>
    /// 创建新列表到看板
    /// </summary>
    private async Task<KeyValuePair<bool, string>> CreateNewList()
    {
        var pair = new KeyValuePair<bool, string>(true, string.Empty);

        foreach (var listName in userListName)
        {
            if (trello.cachedUserLists.ContainsKey(listName) == false)
            {
                var newList = trello.NewList(listName);
                pair = await trello.WebRequest_UploadNewUserList(newList);
                if (pair.Key == false)
                {
                    return pair;
                }
            }
        }

        return pair;
    }

    /// <summary>
    /// 截图
    /// </summary>
    private IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();
        screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        ReportError();
    }
    
    /// <summary>
    /// 上传报错
    /// </summary>
    private async void ReportError()
    {
        var card = trello.NewCard(cardTitle, cardDescription, cardList);
        var pair = await trello.WebRequest_UploadNewUserCard(card);
        var newCardID = pair.Value;
        
        // 上传附件 [截图]
        await trello.WebRequest_UploadAttachmentToCard_Image(newCardID, "ErrorScreenshot.png", screenshot);

        // 上传附件 [字符串]
        await trello.WebRequest_UploadAttachmentToCard_String(newCardID, "ErrorInfo.txt", $"{condition}\r\n{stacktrace}");

        // 上传附件 [文本类文件]
        // await trello.WebRequest_UploadAttachmentToCard_TextFile(newCardID, "这是报错日志.json", "C:\\1.json");
        
        //Game.UI.OpenUI<UI_BugReport>();
        isReporting = false;
    }
    
    /// <summary>
    /// 注册日志收集
    /// </summary>
    private void RegisterLogCollect()
    {
        Application.logMessageReceivedThreaded += ApplicationOnLogMessageReceived;
    }
        
    /// <summary>
    /// 日志收集
    /// </summary>
    private void ApplicationOnLogMessageReceived(string currentCondition, string currentStacktrace, LogType currentType)
    {
        /*
        if (GameConfig.closeErrorLog)
        {
            return;
        }
        if (GameConfig.isMod)
        {
            return;
        }
        */
        
        if (currentType == LogType.Exception || currentType == LogType.Error)
        {
            if (!isReporting)
            {
                isReporting = true;
                cardTitle = "新增异常";
                cardDescription = "平台" + Application.platform + "\n版本" + Application.version + "\n游戏已运行时间" + Time.time;
                cardList = "Bug";
                condition = currentCondition;
                stacktrace = currentStacktrace;
                type = currentType;
                StartCoroutine(CaptureScreenshot());
            }
        }
    }

    // -------------------------------------------测试代码: 生成异常----------------------------------------------------//
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            throw new Exception("sdasdsa Q !");
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            throw new Exception("新增异常 W !");
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            throw new Exception("新增异常 E !");
        }
    }
    */
}
