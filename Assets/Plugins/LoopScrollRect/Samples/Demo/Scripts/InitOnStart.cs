using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
[DisallowMultipleComponent]
public class InitOnStart : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public GameObject item;
    public int totalCount = -1;
    public delegate GameObject UIPoolGet(GameObject go);
    public UIPoolGet uiPoolGetEvent;
    
    // Implement your own Cache Pool here. The following is just for example.
    Stack<Transform> pool = new Stack<Transform>();

    public GameObject GetObject(int index)
    {
        GameObject loadItem;
        if (pool.Count == 0)
        {
            loadItem = Instantiate(item);
        }
        else
        {
            Transform candidate = pool.Pop();
            candidate.gameObject.SetActive(true);
            loadItem = candidate.gameObject;
        }

        if (uiPoolGetEvent != null)
        {
            uiPoolGetEvent(loadItem);
        }
        
        return loadItem;
    }

    public void ReturnObject(Transform trans)
    {
        // Use `DestroyImmediate` here if you don't need Pool
        trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);
        pool.Push(trans);
    }

    public void ProvideData(Transform transform, int idx)
    {
        //transform.SendMessage("ScrollCellIndex", idx);
    }

    void Start()
    {
        var ls = GetComponent<LoopScrollRect>();
        ls.prefabSource = this;
        ls.dataSource = this;
        ls.totalCount = totalCount;
        ls.RefillCells();
    }
}