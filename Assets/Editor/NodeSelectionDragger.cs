using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSelectionDragger : SelectionDragger
{
    private DialogTreeGraphView graphView;
    private static Rect m_SelectionRect;
    private static DragRect dragRect;
    private static bool m_IsDraggingSelection;
    private Vector2 initialMousePosition;

    public NodeSelectionDragger(DialogTreeGraphView gv)
    {
        graphView = gv;

        //Create and style the drag rect
        dragRect = new DragRect();
        graphView.AddElement(dragRect);
        this.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(SelectOnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(SelectOnMouseMove);
        target.RegisterCallback<MouseUpEvent>(SelectOnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(SelectOnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(SelectOnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(SelectOnMouseUp);
    }

    private void SelectOnMouseDown(MouseDownEvent evt)
    {
        // If Shift key is not pressed, do not allow selection dragging
        if (evt.button == (int)MouseButton.LeftMouse)
        {
            ((DialogTreeGraphView)this.target).ClearSelection();
            initialMousePosition = evt.localMousePosition;
            // Start selection dragging
            m_SelectionRect = new Rect(initialMousePosition, Vector2.zero);
            dragRect.style.visibility = Visibility.Visible;
            dragRect.style.top = initialMousePosition.y;
            dragRect.style.left = initialMousePosition.x;
            dragRect.style.bottom = initialMousePosition.y;
            dragRect.style.right = initialMousePosition.x;
            dragRect.style.width = 0;
            dragRect.style.height = 0;
            m_IsDraggingSelection = true;
            evt.StopPropagation();
        }
    }

    private void SelectOnMouseMove(MouseMoveEvent evt)
    {
        if (m_IsDraggingSelection)
        {
            // Update the selection rectangle
            if(initialMousePosition.y > evt.localMousePosition.y)
            {
                dragRect.style.top = evt.localMousePosition.y;
                dragRect.style.bottom = initialMousePosition.y;
                m_SelectionRect.yMin = evt.localMousePosition.y;
                m_SelectionRect.yMax = initialMousePosition.y;
            }
            else
            {
                dragRect.style.top = initialMousePosition.y;
                dragRect.style.bottom = evt.localMousePosition.y;
                m_SelectionRect.yMin = initialMousePosition.y;
                m_SelectionRect.yMax = evt.localMousePosition.y;
            }
            if (initialMousePosition.x > evt.localMousePosition.x)
            {
                dragRect.style.left = evt.localMousePosition.x;
                dragRect.style.right = initialMousePosition.x;
                m_SelectionRect.xMin = evt.localMousePosition.x;
                m_SelectionRect.xMax = initialMousePosition.x;
            }
            else
            {
                dragRect.style.left = initialMousePosition.x;
                dragRect.style.right = evt.localMousePosition.x;
                m_SelectionRect.xMin = initialMousePosition.x;
                m_SelectionRect.xMax = evt.localMousePosition.x;
            }
            dragRect.style.width = dragRect.style.right.value.value - dragRect.style.left.value.value;
            dragRect.style.height = dragRect.style.bottom.value.value - dragRect.style.top.value.value;

            // Mark the graph view as dirty to repaint the selection rect
            ((DialogTreeGraphView)this.target).MarkDirtyRepaint();
            evt.StopPropagation();
        }
    }
    private void SelectOnMouseUp(MouseUpEvent evt)
    {
        if (m_IsDraggingSelection)
        {
            dragRect.style.visibility = Visibility.Hidden;
            // Perform selection of nodes within the selection rectangle
            List<GraphElement> selectedNodes = new List<GraphElement>();
            foreach (var elem in ((DialogTreeGraphView)this.target).graphElements.ToList())
            {
                if (elem is Node node && m_SelectionRect.Overlaps(node.GetPosition()))
                {
                    ((DialogTreeGraphView)this.target).AddToSelection(node);
                }
            }

            // Stop selection dragging
            m_IsDraggingSelection = false;
            evt.StopPropagation();
        }
    }
}
