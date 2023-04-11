using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public partial class DialogTreeGraphView : GraphView
{
    public DialogTreeGraphView()
    {
        //TODO: This node selection dragger may be able to be replaced by some of the out of the box manipulators
        this.AddManipulator(new NodeSelectionDragger(this));
        //this.AddManipulator(new SelectionDragger());
        //this.AddManipulator(new RectangleSelector());


        //TODO: Zooming messes with the selection dragger
        //Zoom - scroll
        this.AddManipulator(new ContentZoomer());
        //Pan - middle mouse
        this.AddManipulator(new ContentDragger());


        style.borderBottomWidth = 1;
        style.borderLeftWidth = 1;
        style.borderRightWidth = 1;
        style.borderTopWidth = 1;
        style.borderBottomColor = Color.white;
        style.borderLeftColor = Color.white;
        style.borderRightColor = Color.white;
        style.borderTopColor = Color.white;
    }

    public List<Port> GetCompatiblePorts(Port startPort)
    {
        List<Port> compatiblePorts = new List<Port>();
        foreach (Port port in ports)
        {
            if(port != startPort && port.node != startPort.node && port.direction != startPort.direction && port.portType == startPort.portType)
            {
                compatiblePorts.Add(port);
            }
        }
        return compatiblePorts;
    }
}
