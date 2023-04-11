using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BooleanNodeData : NodeData
{
    [SerializeField]
    private string connectedNodesAsString;
    [SerializeField]
    private string outputName;
    private List<string> connectedNodeIds;

    public string ConnectedNodesAsString { get => connectedNodesAsString; set => connectedNodesAsString = value; }
    public string OutputName { get => outputName; set => outputName = value; }
    public List<string> ConnectedNodeIds { get => connectedNodeIds; set => connectedNodeIds = value; }

    public BooleanNodeData(string type, string id, string title, string outputName, Rect pos, List<string> connections) : base(type, id, title, pos)
    {
        ConnectedNodeIds = connections;
        OutputName = outputName;
    }

    public void StoreIdsAsString()
    {
        ConnectedNodesAsString = string.Join(",", ConnectedNodeIds);
    }
}
