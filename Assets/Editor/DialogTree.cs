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
    [SerializeField]
    public DialogTreeNode startNode;
    [SerializeField]
    public List<DialogTreeNode> nodes;
    [SerializeField]
    public List<Edge> edges;
}