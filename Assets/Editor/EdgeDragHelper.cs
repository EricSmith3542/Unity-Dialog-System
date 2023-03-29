using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EdgeDragHelper : MouseManipulator
{
    private Port m_DraggedPort;
    private Edge m_Edge;
    private DialogTreeGraphView graphView;
    private const float RECT_DIMENSIONS = 5f;

    public EdgeDragHelper(DialogTreeGraphView gv)
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        graphView = gv;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        //target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        //target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (evt.button == (int)MouseButton.LeftMouse)
        {
            // Get the port that was clicked on
            m_DraggedPort = evt.target as Port;
            if (m_DraggedPort != null)
            {
                // Create a new edge and add it to the graph
                m_Edge = new Edge();
                m_DraggedPort.Connect(m_Edge);

                List<Port> compatiblePorts = graphView.GetCompatiblePorts(m_DraggedPort);
                foreach (Port port in compatiblePorts)
                {
                    port.highlight = true;
                }

                if(m_DraggedPort.direction == Direction.Output)
                {
                    // Set the edge's output port to the dragged port
                    m_Edge.output = m_DraggedPort;
                }
                else
                {
                    m_Edge.input = m_DraggedPort;
                }
                evt.StopPropagation();
            }
        }
    }

    //private void OnMouseMove(MouseMoveEvent evt)
    //{
    //}

    private void OnMouseUp(MouseUpEvent evt)
    {
        if (m_DraggedPort != null)
        {
            // Get the port that was released on
            Port releasedPort = null;
            VisualElement content = graphView.contentContainer;

            // Get the Rect surrounding the current mouse position
            Rect dropZoneRect = new Rect(evt.mousePosition, Vector2.zero);
            dropZoneRect.size = new Vector2(RECT_DIMENSIONS, RECT_DIMENSIONS);

            // Find first port that intersect with the selection rect
            releasedPort = content.Query<Port>()
                .Where(port => dropZoneRect.Overlaps(port.worldBound)).First();


            // If a valid port was released on, create a new edge between the ports
            if (releasedPort != null && releasedPort.direction != m_DraggedPort.direction)
            {
                m_Edge = releasedPort.ConnectTo(m_DraggedPort);
                graphView.AddElement(m_Edge);
            }
            else
            {
                m_DraggedPort.Disconnect(m_Edge);
                m_Edge = null;
            }

            // Reset the dragged port and the edge preview
            m_DraggedPort = null;

            evt.StopPropagation();
        }
    }
}