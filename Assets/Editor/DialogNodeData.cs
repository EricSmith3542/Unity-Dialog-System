using System;
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

    [SerializeField]
    private Color borderColor;
    [SerializeField]
    private Color borderImage;
    [SerializeField]
    private Sprite backGroundColor;
    [SerializeField]
    private Sprite backGroundImage;

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
