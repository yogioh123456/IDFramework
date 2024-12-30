using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    [SerializeField] private string stringKey;
    [SerializeField] private string extraStr;
    private Text myText;
    private string id;

    private void Start()
    {
        if (myText == null)
        {
            myText = GetComponent<Text>();
        }

        if (!string.IsNullOrEmpty(stringKey))
        {
            id = stringKey;
        }
        else
        {
            id = myText.text;
            this.RegisterEvent();
        }
        SetText();
    }

    [EventMsg]
    public void ChangeLanguage()
    {
        SetText();
    }
    
    void SetText()
    {
        string s = Game.Language.Get(id);
        if (s==null)
        {
            Debug.LogError("错误");
        }
        myText.text = Game.Language.Get(s) + extraStr;
    }
}
