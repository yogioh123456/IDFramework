using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonTreeData {
    public List<TreeNode> nodeList = new List<TreeNode>();

    public void AddNode(string name, Action ac) {
        string[] str = name.Split('/');
        List<TreeNode> curListData = nodeList;
        TreeNode parentNode = null;
        for (int i = 0; i < str.Length; i++) {
            TreeNode treeNode = GetNodeByName(curListData, str[i]);
            if (parentNode != null) {
                treeNode.parentNode = parentNode;
            }
            if (i < str.Length - 1) {
                curListData = treeNode.treeNodes;
            } else {
                treeNode.action += () => {
                    ac.Invoke();
                    //ControllerManager.Instance.Close<DevToolController>();
                };
            }
            parentNode = treeNode;
        }
    }

    private TreeNode GetNodeByName(List<TreeNode> list, string name) {
        foreach (var one in list) {
            if (one.name.Equals(name)) {
                return one;
            }
        }
        TreeNode node = new TreeNode(name);
        list.Add(node);
        return node;
    }
}

public class TreeNode {
    public string name;
    public Action action;
    public Button button;
    public PoolView poolView;
    public TreeNode parentNode;//父节点
    public List<TreeNode> treeNodes = new List<TreeNode>();//子节点
    
    public TreeNode(string name) {
        this.name = name;
    }

    public bool ChildNodeIsShow() {
        return poolView != null && poolView.viewList.Count > 0;
    }

    public void CloseChildNode() {
        foreach (var one in treeNodes) {
            one.CloseChildNode();
        }
        poolView?.RemoveAllView();
    }
}
