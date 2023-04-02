using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BooleanNode : DialogTreeNode
{
    public BooleanNode(string title, DialogTreeGraphView gv, string id, Rect pos) : base(title, gv, id, pos)
    {
        AddOutputPort();
    }
    public BooleanNode(string title, DialogTreeGraphView gv, string id) : this(title, gv, id, new Rect(0, 0, 0, 0)) { }
    public BooleanNode(DialogTreeGraphView gv, string id) : this("Start", gv, id) { }

    private void AddOutputPort()
    {
        Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        output.AddManipulator(new EdgeDragHelper(graphView));
        output.portName = "Output";
        output.portColor = Color.cyan;
        outputContainer.Add(output);
    }
}
