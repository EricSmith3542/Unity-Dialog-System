using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeDragger : MouseManipulator
{
    private bool m_Active = false;
    private Vector2 m_Start;
    private List<float> start_lefts, start_tops;
    private DialogTreeGraphView graphView;

    public NodeDragger(DialogTreeGraphView gv)
    {
        graphView = gv;
        this.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    private void OnMouseDown(MouseDownEvent e)
    {
        if (e.button == (int)MouseButton.LeftMouse)
        {
            m_Active = true;
            m_Start = e.mousePosition;

            start_lefts = new List<float>();
            start_tops = new List<float>();
            foreach (Node node in graphView.selection)
            {
                start_lefts.Add(node.style.left.value.value);
                start_tops.Add(node.style.top.value.value);
            }
            target.CaptureMouse();
            e.StopPropagation();
        }
    }

    private void OnMouseMove(MouseMoveEvent e)
    {
        if (m_Active)
        {
            float zoom = graphView.contentViewContainer.transform.scale.x;
            for (int i = 0; i < graphView.selection.Count; i++)
            {
                VisualElement node = (VisualElement)graphView.selection[i];
                node.style.left = start_lefts[i] + ((e.mousePosition.x - m_Start.x) / zoom);
                node.style.top = start_tops[i] + ((e.mousePosition.y - m_Start.y)/ zoom);
            }
            e.StopPropagation();
        }
    }

    private void OnMouseUp(MouseUpEvent e)
    {
        if (e.button == (int)MouseButton.LeftMouse && m_Active)
        {
            m_Active = false;
            target.ReleaseMouse();
            e.StopPropagation();
        }
    }
}
