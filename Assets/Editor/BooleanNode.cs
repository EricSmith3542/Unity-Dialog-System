using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BooleanNode : DialogTreeNode
{
    private static HashSet<string> conditionNames = new HashSet<string>();

    //TODO: Refactor these constructors, should be able to dedupe some code by making AddOutputPort handle 
    public BooleanNode(string title, string portName, DialogTreeGraphView gv, string id, Rect pos) : base(title, gv, id, pos)
    {
        defaultPortPrefix = "Condition ";
        AddOutputPort(portName);
    }
    public BooleanNode(string title, DialogTreeGraphView gv, string id, Rect pos) : this(title, "", gv, id, pos) { }
    public BooleanNode(string title, string portName, DialogTreeGraphView gv, string id) : this(title, portName, gv, id, new Rect(0, 0, 0, 0)) { }
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

    public override void AddOutputPort(string portName)
    {
        Debug.Log(string.Join(" , ", conditionNames));
        if(portName == "")
        {
            AddOutputPort();
        }
        else if (!conditionNames.Contains(portName))
        {
            base.AddOutputPort(portName);
            conditionNames.Add(portName);
        }
        else
        {
            Debug.LogError("Tried to create port with name \"" + portName + "\". Port names on boolean nodes must be unique across all boolean nodes.");
        }
    }

    public override void ChangeName(string oldName, string newName)
    {
        if(oldName == newName) { return; }
        conditionNames.Remove(oldName);
        conditionNames.Add(newName);
    }

    public override bool IsUniqueOutputName(string name)
    {
        return !conditionNames.Contains(name);
    }

    public static void RefreshPortNameSet()
    {
        conditionNames = new HashSet<string>();
    }
}
