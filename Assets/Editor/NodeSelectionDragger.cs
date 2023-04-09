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
        if (evt.button == (int)MouseButton.LeftMouse && evt.target is DialogTreeGraphView)
        {
            //Clear the current selection
            ((DialogTreeGraphView)this.target).ClearSelection();

            //Get the mouse position relative to the pan/zoom of the graphview
            initialMousePosition = graphView.contentViewContainer.WorldToLocal(evt.mousePosition);

            //Create and visualize selection rectangle
            m_SelectionRect = new Rect(initialMousePosition, Vector2.zero);
            dragRect.style.visibility = Visibility.Visible;
            dragRect.SetPosition(new Rect(initialMousePosition, Vector2.zero));
            m_IsDraggingSelection = true;

            evt.StopPropagation();
        }
    }

    private void SelectOnMouseMove(MouseMoveEvent evt)
    {
        if (m_IsDraggingSelection)
        {
            //Get the mouse position relative to the pan/zoom of the graphview
            Vector2 translatedPos = graphView.contentViewContainer.WorldToLocal(evt.mousePosition);

            //Calculate new rectangle size and positon
            float x1 = Mathf.Min(initialMousePosition.x, translatedPos.x);
            float y1 = Mathf.Min(initialMousePosition.y, translatedPos.y);
            float x2 = Mathf.Max(initialMousePosition.x, translatedPos.x);
            float y2 = Mathf.Max(initialMousePosition.y, translatedPos.y);
            float width = Mathf.Abs(x2 - x1);
            float height = Mathf.Abs(y2 - y1);

            //Redraw the rectangle
            m_SelectionRect = new Rect(x1, y1, width, height);
            dragRect.SetPosition(m_SelectionRect);

            // Mark the graph view as dirty to repaint the selection rect
            ((DialogTreeGraphView)this.target).MarkDirtyRepaint();
            evt.StopPropagation();
        }
    }
    private void SelectOnMouseUp(MouseUpEvent evt)
    {
        if (m_IsDraggingSelection)
        {
            // Perform selection of nodes within the selection rectangle
            foreach (var elem in ((DialogTreeGraphView)this.target).graphElements.ToList())
            {
                if (elem is Node node && m_SelectionRect.Overlaps(node.GetPosition()))
                {
                    ((DialogTreeGraphView)this.target).AddToSelection(node);
                }
            }

            // Stop selection dragging
            dragRect.style.visibility = Visibility.Hidden;
            m_IsDraggingSelection = false;
            evt.StopPropagation();
        }
    }
}
