using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BooleanNode : DialogTreeNode
{
    public BooleanNode(string title, string portName, DialogTreeGraphView gv, string id, Rect pos) : base(title, gv, id, pos)
    {
        defaultPortPrefix = "Condition ";
        AddOutputPort(portName);
    }
    public BooleanNode(string title, DialogTreeGraphView gv, string id, Rect pos) : base(title, gv, id, pos)
    {
        defaultPortPrefix = "Condition ";
        AddOutputPort();
    }
    public BooleanNode(string title, DialogTreeGraphView gv, string id) : this(title, gv, id, new Rect(0, 0, 0, 0)) { }
    public BooleanNode(DialogTreeGraphView gv, string id) : this("Start", gv, id) { }

    public override NodeData AsData()
    {
        Port output = outputContainer.Query<Port>().First();

        List<string> connectedNodeIds = new List<string>();
        foreach (Edge edge in output.connections)
        {
            if(edge.input != null)
            {
                connectedNodeIds.Add(edge.input.GetFirstAncestorOfType<DialogNode>().id);
            }
        }
        return new BooleanNodeData(id, nodeTitle, EditableLabel.FetchEditableLabel(output).text, GetPosition(), connectedNodeIds);
    }
}
