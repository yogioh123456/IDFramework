using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolView : MonoBehaviour
{
    public GameObject target;
    [HideInInspector]
    public List<GameObject> viewList = new List<GameObject>();
    List<GameObject> poolList = new List<GameObject>();
    private GameObject _pool;

    public GameObject Pool
    {
        get
        {
            if (_pool == null)
            {
                GameObject go = new GameObject();
                go.transform.SetParent(transform);
                go.SetZero();
                go.name = "_pool";
                _pool = go;
            }
            return _pool;
        }
    }

    private void Start()
    {
        if (target != null)
        {
            target.transform.SetParent(Pool.transform);
            target.SetActive(false);
            poolList.Add(target);
        }
    }

    public GameObject AddView()
    {
        GameObject _view;
        if (poolList.Count > 1)
        {
            _view = poolList[0];
            poolList.Remove(_view);
            _view.SetActive(true);
            _view.transform.SetParent(transform);
        }
        else
        {
            _view = Instantiate(target, transform, false);
        }
        _view.SetActive(true);
        viewList.Add(_view);
        _view.SetZero();
        return _view;
    }

    public void RemoveView(GameObject go)
    {
        go.transform.SetParent(Pool.transform);
        go.SetActive(false);
        poolList.Add(go);
        viewList.Remove(go);
    }

    public void RemoveAllView()
    {
        for (int i = viewList.Count; i > 0; i--)
        {
            //嵌套的PoolView清理
            PoolView[] childPools = viewList[0].GetComponentsInChildren<PoolView>();
            foreach (var childPool in childPools)
            {
                childPool.RemoveAllView();
            }
            RemoveView(viewList[0]);
        }
    }
}
