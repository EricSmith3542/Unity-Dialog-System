using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DialogPort : Port
{
    public DialogPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type)
        : base(portOrientation, portDirection, portCapacity, type)
    {
        RegisterCallback<MouseDownEvent>(OnStartEdgeDragging);
    }

    public void OnStartEdgeDragging(MouseDownEvent evt)
    {
        base.OnStartEdgeDragging();
    }

    public override void OnSelected()
    {
        base.OnSelected();
    }

}
