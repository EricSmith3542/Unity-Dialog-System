using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogTree : ScriptableObject
{
    [SerializeField]
    public string treeName;
    [SerializeField]
    public string characterId;
    [SerializeReference]
    public string startNodeId;
    [SerializeReference]
    public List<NodeData> nodes;
}