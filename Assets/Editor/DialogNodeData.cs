using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class DialogNodeData : NodeData
{
    public DialogNodeData(DialogNode node) : base(node.id, node.nodeTitle, node.GetPosition())
    {
        dialog = node.Dialog;
        borderColor = node.GetBorderColor();
        borderImage = node.GetBorderSprite();
        backGroundColor = node.GetBackgroundColor();
        backGroundImage = node.GetBackgroundSprite();

        SerializeableMap connections = new SerializeableMap();
        foreach (Port outputPort in node.outputContainer.Query<Port>().ToList())
        {
            List<string> inputIds = new List<string>();
            foreach (Edge edge in outputPort.connections)
            {
                inputIds.Add((edge.input.GetFirstAncestorOfType<DialogTreeNode>()).id);
            }
            connections.Add(EditableLabel.FetchEditableLabel(outputPort).text, inputIds);
        }
        outputPortsConnectionsMap = connections;
    }
    [SerializeField]
    private string dialog;

    [SerializeField]
    private Color borderColor;
    [SerializeField]
    private Sprite borderImage;

    [SerializeField]
    private Color backGroundColor;
    [SerializeField]
    private Sprite backGroundImage;

    [SerializeReference]
    public SerializeableMap outputPortsConnectionsMap;

    public override DialogTreeNode AsNode(DialogTreeGraphView gv)
    {
        DialogNode node = new DialogNode(title, gv, id, pos);
        node.Dialog = dialog;

        //Remove the starting port
        node.RemoveOutputPort();
        outputPortsConnectionsMap.RecreateValuesListFromString();
        foreach (string outputPortName in outputPortsConnectionsMap.Keys)
        {
            node.AddOutputPort(outputPortName);
        }

        node.SetBorderColor(borderColor);
        node.SetBorderSprite(borderImage);
        node.SetBackgroundColor(backGroundColor);
        node.SetBackgroundSprite(backGroundImage);

        return node;
    }
}
