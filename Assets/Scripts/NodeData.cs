using System;
using UnityEngine;

[Serializable]
public abstract class NodeData
{
    public NodeData(string type, string id, string title, Rect pos)
    {
        Id = id;
        Title = title;
        Position = pos;
        NodeType = type;
    }

    [SerializeField]
    private string nodeType;
    [SerializeField]
    private string id;
    [SerializeField]
    private string title;
    [SerializeField]
    private Rect position;

    public string NodeType { get => nodeType; set => nodeType = value; }
    public string Id { get => id; set => id = value; }
    public string Title { get => title; set => title = value; }
    public Rect Position { get => position; set => position = value; }
}