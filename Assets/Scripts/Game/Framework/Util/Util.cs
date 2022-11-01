using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RandomData
{
    public System.Random random;
    public int count;
    public int hash;

    public RandomData()
    {
        random = new System.Random();
    }
    
    public RandomData(int hash, int count = 0)
    {
        this.hash = hash;
        this.count = count;
        random = new System.Random(hash);
        for (int i = 0; i < count; i++)
        {
            random.Next();
        }
    }

    public int Next(int min, int max)
    {
        count++;
        return random.Next(min, max);
    }
}

public static class Util {
    
    public static Transform SetZero(this Transform go)
    {
        go.localPosition = Vector3.zero;
        go.localScale = Vector3.one;
        go.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        return go;
    }
    public static GameObject SetZero(this GameObject go)
    {
        return SetZero(go.transform.SetZero()).gameObject;
    }
    public static GameObject SetPos(this GameObject go,float x, float y, float z)
    {
        go.transform.localPosition = new Vector3(x, y, z);
        go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        return go;
    }
    public static GameObject SetPos(this GameObject go,Vector3 v3)
    {
        go.transform.localPosition = v3;
        go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        return go;
    }

    //延迟执行
    public async static void DelayAction(Action ac, int time)
    {
        //1000为1秒
        await Task.Delay(time);
        ac();
    }

    public static bool IsTouchUI()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return true;
            }
        }
#else
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
#endif
        return false;
    }
    
    /// <summary>
    /// 将对象转换为byte数组
    /// </summary>
    /// <param name="obj">被转换对象</param>
    /// <returns>转换后byte数组</returns>
    public static byte[] Object2Bytes(this object obj)
    {
        byte[] buff;
        using (MemoryStream ms = new MemoryStream())
        {
            IFormatter iFormatter = new BinaryFormatter();
            iFormatter.Serialize(ms, obj);
            buff = ms.GetBuffer();
        }
        return buff;
    }

    /// <summary>
    /// 将byte数组转换成对象
    /// </summary>
    /// <param name="buff">被转换byte数组</param>
    /// <returns>转换完成后的对象</returns>
    public static object Bytes2Object(this byte[] buff)
    {
        object obj;
        using (MemoryStream ms = new MemoryStream(buff))
        {
            IFormatter iFormatter = new BinaryFormatter();
            obj = iFormatter.Deserialize(ms);
        }
        return obj;
    }
    
    /// <summary>
    /// string转IPEndPoint
    /// 在.net core平台上，将IPEndPoint序列化发生了问题，所以转字符串了
    /// </summary>
    /// <param name="ipStr"></param>
    /// <returns></returns>
    public static IPEndPoint String2IP(this string ipStr)
    {
        string[] array = ipStr.Split(':');
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(array[0]), Int32.Parse(array[1]));
        return ip;
    }

    /// <summary>
    /// 辅助ExcelManager全字典式数据
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static object DIC(this object dic, string key)
    {
        if (((Dictionary<string, object>) dic).ContainsKey(key))
        {
            return ((Dictionary<string, object>) dic)[key];//字典索引返回的可能是一个新的对象，不是之前的引用，如果改变值是不能改变之前的值，可以用下面的方法
        }

        return null;
    }

    public static T DIC<T>(this object dic, string key)
    {
        if (((Dictionary<string, object>) dic).ContainsKey(key))
        {
            object data = ((Dictionary<string, object>) dic)[key];
            object newData = data;
            
            switch (typeof(T).ToString())
            {
                case "System.Int32":
                    newData = data.ToInt32();
                    break;
                case "System.String":
                    newData = data.ToString();
                    break;
                case "System.Boolean":
                    newData = Convert.ToBoolean(data);
                    break;
                case "System.Single":
                    newData = Convert.ToSingle(data);
                    break;
                case "System.Double":
                    newData = Convert.ToDouble(data);
                    break;
            }
            
            return (T) newData;
        }

        return default;
    }
    
    public static Dictionary<string, object> DIC(this object dic)
    {
        return ((Dictionary<string, object>) dic);
    }
    
    /// <summary>
    /// 万能克隆，深度克隆，字典和List可以直接.Clone，如果是类的话那个类必须得打上[Serializable]标签
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="RealObject"></param>
    /// <returns></returns>
    public static T Clone<T>(this T RealObject)
    {
        using (Stream objectStream = new MemoryStream())
        {
            //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制  
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, RealObject);
            objectStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(objectStream);
        }
    }

    //方便地转int32类型
    public static int ToInt32(this object data)
    {
        if (data == null)
        {
            return 0;
        }
        if (string.IsNullOrEmpty(data.ToString()))
        {
            return 0;
        }
        return Convert.ToInt32(data);
    }

    //隐转数组
    public static object[] ToObjectArray(this object dataArray)
    {
        return (object[])dataArray;
    }

    //方便地判断是不是空字符串
    public static bool IsEmptyOrNull(this string data)
    {
        return string.IsNullOrEmpty(data);
    }

    //如果数字为1的话，则返回空字符串
    public static string ToCheckZero(this int num)
    {
        if (num == 1)
        {
            return "";
        }
        return num.ToString();
    }

    //判断自己有没有这个组件，没有的话自动添加个
    public static T GetComponentAutoAdd<T>(this GameObject go) where T : Component
    {
        if (go.GetComponent<T>() == null)
        {
            return go.AddComponent<T>();
        }
        else
        {
            return go.GetComponent<T>();
        }
    }
    public static T GetComponentAutoAdd<T>(this Transform go) where T : Component
    {
        if (go.GetComponent<T>() == null)
        {
            return go.gameObject.AddComponent<T>();
        }
        else
        {
            return go.GetComponent<T>();
        }
    }

    //布尔转int
    public static int ToBool2Int(this bool b)
    {
        if (b)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// 添加唯一动画事件
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="newEvent"></param>
    public static void AddEventOnlyOne(this AnimationClip clip, AnimationEvent newEvent)
    {
        bool isAdd = false;
        for (int i = 0 ;i < clip.events.Length; i++)
        {
            if (clip.events[i].functionName.Equals(newEvent.functionName))
            {
                Debug.LogWarning("动画事件添加过了，或者添加了相同名字的事件");
                clip.events[i] = newEvent;
                isAdd = true;
            }
        }

        if (!isAdd)
        {
            clip.AddEvent(newEvent);
        }
    }

    public static Vector2 GetCenterToLB(this Vector3 v2, float width, float height)
    {
        return new Vector2(v2.x - (width / 2), v2.y - (height / 2));
    }

    public static void LookAt2D(this Transform transform, Transform target, int flipDir)
    {
        Vector3 dir = (target.position - transform.position) * flipDir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime);
    }

    public static void AddToggleEvent(this Toggle toggle, Action ac)
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                ac();
            }
        });
    }

    public static int GetNum(this Text _text)
    {
        return _text.text.ToInt32();
    }
    
    public static void SetNum(this Text _text, int _num)
    {
        _text.text = _num.ToString();
    }
    
    public static void AddNum(this Text _text, int _num)
    {
        int result = _text.text.ToInt32();
        result += _num;
        _text.text = result.ToString();
    }
    
    public const int sortingOrderDefault = 5000;
    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    
    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static int GetAbsoluteDistance(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }
    
    /// <summary>
    /// 数组打乱，洗牌算法
    /// </summary>
    /// <param name="original"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> Shuffle<T>(this List<T> original)
    {
        RandomData randomNum = new RandomData();
        return original.Shuffle(randomNum);
    }
    
    public static List<T> Shuffle<T>(this List<T> original, RandomData randomNum)
    {
        int index = 0;
        T temp;
        for (int i = 0; i < original.Count; i++)
        {
            index = randomNum.Next(0, original.Count - 1);
            if (index != i)
            {
                temp = original[i];
                original[i] = original[index];
                original[index] = temp;
            }
        }
        return original;
    }

    /// <summary>
    /// 从List里面选择不重复的num个元素
    /// </summary>
    /// <param name="original"></param>
    /// <param name="randomNum"></param>
    /// <param name="num"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> GetRandomNumList<T>(this List<T> original, RandomData randomData, int num)
    {
        if (num >= original.Count)
        {
            return original;
        }
        
        List<int> tempList = new List<int>();
        List<T> dataList = new List<T>();
        for (int i = 0; i < original.Count; i++)
        {
            tempList.Add(i);
        }
        int count = 0;
        while (count < num && tempList.Count > 0)
        {
            int index = randomData.Next(0, tempList.Count);
            var _value = original[tempList[index]];
            if (!dataList.Contains(_value))
            {
                dataList.Add(_value);
                tempList.RemoveAt(index);
                count++;
            }
        }
        return dataList;
    }

    /// <summary>
    /// 文件夹复制
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="destPath"></param>
    public static void CopyFolder(string sourcePath, string destPath)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(destPath))
            {
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    Debug.LogError("创建失败" + ex.Message);
                }
            }
            
            List<string> files = new List<string>(Directory.GetFiles(sourcePath));
            files.ForEach(c =>
            {
                //排除meta文件
                if (!c.EndsWith(".meta"))
                {
                    string destFile = Path.Combine(destPath, Path.GetFileName(c));
                    File.Copy(c, destFile, true);
                }
            });
            List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
            folders.ForEach(c =>
            {
                string destDir = Path.Combine(destPath,Path.GetFileName(c));
                CopyFolder(c, destDir);
            });
        }
        else
        {
            Debug.LogError("源目录不存在");
        }
    }

    /// 获取文件的MD5码  
    /// </summary>  
    /// <param name="fileName">传入的文件名（含路径及后缀名）</param>  
    /// <returns></returns>  
    public static string GetMD5HashFromFile(byte[] file)
    {
        try
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }

    /// <summary>
    /// 需要配合Camera.main.ScreenPointToRay(Input.mousePosition);使用检测位置合法性
    /// </summary>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static bool CheckInputPositionIsLarge(Vector2 v2)
    {
        if (!Camera.main.pixelRect.Contains(v2))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 二维数组越界
    /// </summary>
    /// <param name="t"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool CheckArray2Out<T>(this T[,] t,int x, int y)
    {
        if (x < 0 || y < 0)
        {
            return true;
        }
        
        if (x >= t.GetLength(0) || y >= t.GetLength(1))
        {
            return true;
        }

        return false;
    }
    
    public static int GetWeightIndex(string[] array)
    {
        int[] data = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            data[i] = array[i].ToInt32();
        }

        return GetWeightIndex(data);
    }

    public static int GetWeightIndex(int[] array)
    {
        int all = 0;
        foreach (var one in array)
        {
            all += one;
        }

        int num = UnityEngine.Random.Range(0, all) + 1;
        int sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
            if (num <= sum)
            {
                return i;
            }
        }

        return 0;
    }
    
    /// <summary>
    /// 获取当前的时间戳（精确到秒）
    /// TimeTool.ConvertDateTimep(DateTime.Now)
    /// </summary>
    /// <param name="time">时间</param>
    public static long GetNowTimeTick()
    {
        return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
    }

    /// <summary>
    /// 时间戳转为C#格式时间
    /// TimeTool.GetTime(TimeTool.ConvertDateTiemp(DateTime.Now).ToString())
    /// </summary>
    /// <param name="timeStamp">时间戳</param>
    /// <returns></returns>
    public static DateTime GetTime(this long tick)
    {
        string timeStamp = tick.ToString();
        if (timeStamp.Length > 10)
        {
            timeStamp = timeStamp.Substring(0, 10);
        }
        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dateTimeStart.Add(toNow);
    }
}
