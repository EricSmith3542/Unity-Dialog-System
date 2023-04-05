using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BooleanNodeData : NodeData
{
    public BooleanNodeData(string id, string title, Rect pos, List<string> connections) : base(id, title, pos)
    {
        connectedNodeIds = connections;
    }
    [SerializeField]
    public string connectedNodesAsString;

    public List<string> connectedNodeIds;

    public override DialogTreeNode AsNode(DialogTreeGraphView gv)
    {
        BooleanNode node = new BooleanNode(title, gv, id, pos);
        connectedNodeIds = new List<string>(connectedNodesAsString.Split(","));
        return node;
    }

    public void StoreIdsAsString()
    {
        connectedNodesAsString = string.Join(",", connectedNodeIds);
    }
}
