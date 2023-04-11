using System;
using UnityEngine;

[Serializable]
public abstract class NodeData
{
    public NodeData(string id, string title, Rect pos)
    {
        this.id = id;
        this.title = title;
        this.pos = pos;
    }

    [SerializeField]
    public string id;
    [SerializeField]
    public string title;
    [SerializeField]
    protected Rect pos;

    public abstract DialogTreeNode AsNode();
}