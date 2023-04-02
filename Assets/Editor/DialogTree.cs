using System.Collections.Generic;
using UnityEngine;

public class DialogTree : ScriptableObject
{
    public string treeName;
    public string characterId;
    public DialogTreeNode startNode;
    public List<DialogTreeNode> nodes;
}