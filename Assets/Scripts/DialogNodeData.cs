using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class DialogNodeData : NodeData
{
    public DialogNodeData(string type, string id, string title, Rect pos, string displayName, string dialog, Color borderColor, Sprite borderSprite, Color backgroundColor, Sprite backgroundSprite, SerializeableMap connections) : base(type, id, title, pos)
    {
        Dialog = dialog;
        DisplayName = displayName;
        BorderColor = borderColor;
        BorderImage = borderSprite;
        BackGroundColor = backgroundColor;
        BackGroundImage = backgroundSprite;
        OutputPortsConnectionsMap = connections;
    }
    [SerializeField]
    private string dialog;
    [SerializeField]
    private string displayName;
    [SerializeField]
    private Color borderColor;
    [SerializeField]
    private Sprite borderImage;

    [SerializeField]
    private Color backGroundColor;
    [SerializeField]
    private Sprite backGroundImage;

    [SerializeReference]
    private SerializeableMap outputPortsConnectionsMap;

    public string Dialog { get => dialog; set => dialog = value; }
    public Color BorderColor { get => borderColor; set => borderColor = value; }
    public Sprite BorderImage { get => borderImage; set => borderImage = value; }
    public Color BackGroundColor { get => backGroundColor; set => backGroundColor = value; }
    public Sprite BackGroundImage { get => backGroundImage; set => backGroundImage = value; }
    public SerializeableMap OutputPortsConnectionsMap { get => outputPortsConnectionsMap; set => outputPortsConnectionsMap = value; }
    public string DisplayName { get => displayName; set => displayName = value; }

    //public override DialogTreeNode AsNode()
    //{

    //}
}
