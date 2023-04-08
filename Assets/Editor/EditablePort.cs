using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EditablePort : Port
{
    private Label m_Label;
    private TextField m_EditField;
    private bool m_IsEditing;

    public EditablePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type)
        : base(portOrientation, portDirection, portCapacity, type)
    {
        m_Label = this.Q<Label>("type");
        m_Label.AddManipulator(new Clickable(OnLabelClicked));
        m_EditField = new TextField { isDelayed = true, visible = false };
        m_EditField.RegisterCallback<FocusOutEvent>(OnEditFieldFocusOut);
        m_EditField.RegisterCallback<KeyDownEvent>(OnEditFieldKeyDown);
        this.Add(m_EditField);
    }

    private void OnLabelClicked()
    {
        m_Label.visible = false;
        m_EditField.value = m_Label.text;
        m_EditField.visible = true;
        m_EditField.Focus();
        m_IsEditing = true;
    }

    private void OnEditFieldFocusOut(FocusOutEvent evt)
    {
        if (m_IsEditing)
        {
            m_IsEditing = false;
            m_Label.text = m_EditField.value;
            m_Label.visible = true;
            m_EditField.visible = false;
        }
    }

    private void OnEditFieldKeyDown(KeyDownEvent evt)
    {
        if (evt.keyCode == KeyCode.Escape || evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
        {
            m_IsEditing = false;
            m_Label.text = m_EditField.value;
            m_Label.visible = true;
            m_EditField.visible = false;
        }
    }
}





