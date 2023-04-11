using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BooleanNodeData : NodeData
{
    [SerializeField]
    public string connectedNodesAsString;
    [SerializeField]
    public string outputName;
    public List<string> connectedNodeIds;

    public BooleanNodeData(string id, string title, string outputName, Rect pos, List<string> connections) : base(id, title, pos)
    {
        connectedNodeIds = connections;
        this.outputName = outputName;
    }

    public override DialogTreeNode AsNode()
    {
        BooleanNode node = new BooleanNode(title, outputName, id, pos);
        connectedNodeIds = new List<string>(connectedNodesAsString.Split(","));
        return node;
    }

    public void StoreIdsAsString()
    {
        connectedNodesAsString = string.Join(",", connectedNodeIds);
    }
}
