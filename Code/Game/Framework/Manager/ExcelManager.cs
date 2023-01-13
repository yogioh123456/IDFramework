using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using UnityEngine;

/// <summary>
/// excel管理器
/// excel格式，第一行为key  第二行类型，第三行默认值，第四行参数，第五行注释
/// </summary>
public class ExcelManager
{
    //所有字典
    public static Dictionary<string, object> allExcelData = new Dictionary<string, object>();

    //excel开头非数据行数
    private static int notDataLines = 5;

    public ExcelManager()
    {
        LoadAllData();
        //test
        //Debug.Log(allExcelData["config"].DIC("welcome").DIC<string>("name"));
        //Debug.Log(allExcelData["level"].DIC("1").DIC<string>("info"));
    }

    public object GetData(string tableName)
    {
        return allExcelData[tableName];
    }
    
    private void LoadAllData()
    {
        allExcelData.Clear();
        if (Launch.GameMode == GameMode.Develop)
        {
#if UNITY_EDITOR
            //编辑器模式下读取.Excel的文件
            LoadExcelData(Path.Combine(Application.dataPath, ".Excel"));
#endif
        }
        else
        {
            //发布模式
            LoadExcelConfig();
        }

        GC.Collect();
    }

    private void LoadExcelConfig() {
        var data = Launch.assetBundle.LoadAsset<SOExcelConfig>("SOExcelConfig");
        foreach (var one in data.configs) {
            LoadAbExcel(one);
        }
    }
    
    private void LoadAbExcel(string name)
    {
        byte[] file = Launch.assetBundle.LoadAsset<TextAsset>(name).bytes;
        MemoryStream memStream = new MemoryStream();
        memStream.Write(file, 0, file.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        LoadStream(memStream);
    }

    private void LoadExcelData(string path)
    {
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] fileDic = root.GetFiles();
        foreach (var file in fileDic)
        {
            //查找xlsx，并且开头不是~(打开的文件)
            if (file.FullName.EndsWith("xlsx") && !file.FullName.StartsWith("~"))
            {
                GameReadExcel(file);
            }
        }
    }

    private void GameReadExcel(FileInfo file)
    {
        FileStream stream = file.Open(FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelDataReader.AsDataSet();
        foreach (DataTable one in result.Tables)
        {
            ReadOneTable(one);
        }
    }

    public void LoadStream(MemoryStream stream)
    {
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelDataReader.AsDataSet();
        foreach (DataTable one in result.Tables)
        {
            ReadOneTable(one);
        }
    }


    private void ReadOneTable(DataTable _exceltable)
    {
        //排除以下划线开头的子表
        if (_exceltable.TableName.StartsWith("_"))
        {
            return;
        }

        //字典
        Dictionary<string, object> dic = new Dictionary<string, object>();
        //键值
        List<string> list = new List<string>();
        int cols = _exceltable.Columns.Count;
        int rows = _exceltable.Rows.Count;
        //默认值
        List<string> defaultList = new List<string>();

        //一行一行地读取
        for (int i = 0; i < rows; i++)
        {
            Dictionary<string, object> _cellDic = new Dictionary<string, object>();
            string lineKey = "";
            for (int j = 0; j < cols; j++)
            {
                //数据表中的值
                object v = _exceltable.Rows[i][j];
                //Debug.Log(v.ToString() + "---" + v.GetType());
                //第一行为键值
                if (i == 0)
                {
                    list.Add(v.ToString());
                }
                else if (i == 1)
                {
                }
                else if (i == 2)
                {
                    //第三行为参数默认值
                    defaultList.Add(v.ToString());
                }
                else if (i == 3)
                {
                }
                else if (i == 4)
                {
                }
                else
                {
                    if (!string.IsNullOrEmpty(list[j]))
                    {
                        //如果数据表格中没填值
                        if (v == DBNull.Value)
                        {
                            //默认值为DBNull，这种类型不支持数据存储，需要转化
                            v = defaultList[j];
                            //v = "";
                        }

                        _cellDic.Add(list[j], v);
                    }
                }

                //排除前3行，用id作为每行的key
                if (i >= notDataLines && list[j] == "id")
                {
                    lineKey = v.ToString();
                }
            }

            //数据从第四行开始的
            if (i >= notDataLines)
            {
                if (!string.IsNullOrEmpty(lineKey))
                {
                    if (dic.ContainsKey(lineKey))
                    {
                        Debug.LogError("sss" + lineKey);
                    }

                    dic.Add(lineKey, _cellDic);
                }
            }
        }

        allExcelData.Add(_exceltable.TableName, dic);
        //Debug.Log("添加数据表" + _exceltable.TableName);
    }
}