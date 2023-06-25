using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UI_DevTool : UGUICtrl
{
    public UI_DevTool_View selfView;
    private ButtonTreeData buttonTreeData = new ButtonTreeData();
    private List<TreeNode> curTreeList;
    private TreeNode curTreeNode;

    public UI_DevTool()
    {
        selfView = new UI_DevTool_View();
        OnCreate(selfView,"UI/Prefabs/UI_DevTool",GetType());
        SetData();
    }
    
    private void SetData() {
        Assembly assembly = GetType().Assembly;
        List<Type> typeList = new List<Type>();
        Type[] types = assembly.GetTypes();
        foreach (var one in types) {
            if (one.GetCustomAttribute<DevPriority>() != null) {
                typeList.Add(one);
            }
        }
        typeList.Sort(ComparaList);
        for (int i = 0; i < typeList.Count; i++) {
            Type item = typeList[i];
            MethodInfo[] method = item.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (var methodOne in method) {
                DevConsole data = methodOne.GetCustomAttribute<DevConsole>();
                if (data != null) {
                    buttonTreeData.AddNode(data.name, () => {
                        methodOne.Invoke(null, null);
                    });
                }
            }
        }
        curTreeList = buttonTreeData.nodeList;

        //生成UI
        PoolView content = CreateContent(selfView.point.position).GetComponent<PoolView>();
        CreateUI(content, buttonTreeData.nodeList);
    }

    private int ComparaList(Type t1, Type t2) {
        return t1.GetCustomAttribute<DevPriority>().priority.CompareTo(t2.GetCustomAttribute<DevPriority>().priority);
    }
    
    public override void Update() {
        if (Input.anyKeyDown) {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(keyCode)) {
                    string content = keyCode.ToString(); //Comma Period
                    if (content.Equals("Backspace") || content.Equals("KeypadMinus")) {
                        if (curTreeNode != null) {
                            curTreeNode.CloseChildNode();
                            if (curTreeNode.parentNode != null) {
                                curTreeNode = curTreeNode.parentNode;
                                curTreeList = curTreeNode.treeNodes;
                            } else {
                                curTreeNode = null;
                                curTreeList = buttonTreeData.nodeList;
                            }
                        } else {
                            ClosePanel();
                        }

                        return;
                    }

                    content = content.Replace("Alpha", "");
                    content = content.Replace("Keypad", "");
                    string RegStr = "^[0-9]$";
                    Regex rg = new Regex(RegStr);
                    Match SearchStr = rg.Match(content);
                    content = SearchStr.ToString();
                    if (!string.IsNullOrEmpty(content)) {
                        int index = Int32.Parse(content);
                        if (index < curTreeList.Count) {
                            curTreeList[index].button.onClick.Invoke();
                        }
                    }
                }
            }
        }
    }

    private PoolView CreateContent(Vector3 pos) {
        GameObject go = selfView.contentPool.AddView();
        go.transform.position = pos;
        return go.GetComponent<PoolView>();
    }

    private void CreateUI(PoolView pool, List<TreeNode> nodeList) {
        for (int i = 0; i < nodeList.Count; i++) {
            TreeNode node = nodeList[i];
            GameObject go = pool.AddView();
            go.transform.GetComponentInChildren<Text>().text = i + node.name;
            node.button = go.transform.GetComponent<Button>();
            go.transform.GetComponent<DevToolButton>().action = () => {
                OpenNode(node, nodeList, go);
            };
            node.button.onClick.AddListener(() => {
                if (OpenNode(node, nodeList, go)) {
                    node.action.Invoke();
                }
            });
        }
    }

    private bool OpenNode(TreeNode node, List<TreeNode> nodeList, GameObject go) {
        curTreeNode = node;
        //关闭其他分支节点
        foreach (var other in nodeList) {
            if (node != other) {
                other.CloseChildNode();
            }
        }

        if (node.treeNodes.Count > 0) {
            if (node.ChildNodeIsShow()) {
                node.CloseChildNode();
                curTreeList = nodeList;
            } else {
                //打开分级节点
                PoolView content = CreateContent(go.transform.position + new Vector3(200, 0, 0)).GetComponent<PoolView>();
                node.poolView = content;
                curTreeList = node.treeNodes;
                CreateUI(content, node.treeNodes);
            }
        } else {
            return true;
        }

        return false;
    }
}
