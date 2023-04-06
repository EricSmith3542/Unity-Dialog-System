using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogNodeData : NodeData
{
    public DialogNodeData(string id, string title, Rect pos, SerializeableMap outputConnectionMap, string dialog) : base(id, title, pos)
    {
        outputPortsConnectionsMap = outputConnectionMap;
        this.dialog = dialog;
    }
    [SerializeField]
    private string dialog;
    [SerializeReference]
    public SerializeableMap outputPortsConnectionsMap;

    public override DialogTreeNode AsNode(DialogTreeGraphView gv)
    {
        DialogNode node = new DialogNode(title, gv, id, pos);
        node.Dialog = dialog;

        //Remove the port
        node.outputContainer.RemoveAt(2);
        outputPortsConnectionsMap.RecreateValuesListFromString();
        foreach (string outputPortName in outputPortsConnectionsMap.Keys)
        {
            node.AddOutputPort(outputPortName);
        }
        return node;
    }
}
