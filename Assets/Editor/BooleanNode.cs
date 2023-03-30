using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BooleanNode : DialogTreeNode
{
    public BooleanNode(string title, DialogTreeGraphView gv, Rect pos) : base(title, gv, pos)
    {
        AddOutputPort();
    }
    public BooleanNode(string title, DialogTreeGraphView gv) : this(title, gv, new Rect(0, 0, 0, 0)) { }
    public BooleanNode(DialogTreeGraphView gv) : this("Start", gv) { }

    private void AddOutputPort()
    {
        Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        output.AddManipulator(new EdgeDragHelper(graphView));
        output.portName = "Output";
        output.portColor = Color.cyan;
        outputContainer.Add(output);
    }
}
