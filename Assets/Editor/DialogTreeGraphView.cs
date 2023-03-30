using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public partial class DialogTreeGraphView : GraphView
{
    public DialogTreeGraphView()
    {
        // Add selection dragger to the view
        var selectionDragger = new NodeSelectionDragger(this);
        this.AddManipulator(selectionDragger);        
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
