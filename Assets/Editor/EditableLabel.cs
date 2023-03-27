using UnityEngine.UIElements;

public class EditableLabel : Label
{
    private TextField textField;

    public EditableLabel(string labelText) : base(labelText)
    {
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<ChangeEvent<string>>(OnLabelChanged);
        style.alignSelf = Align.Center;
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (evt.clickCount == 2)
        {
            textField = new TextField { value = text };
            textField.RegisterCallback<BlurEvent>(OnTextFieldBlur);
            Add(textField);
            RemoveFromClassList("editable-label");
            textField.SelectAll();
        }
    }

    private void OnTextFieldBlur(BlurEvent evt)
    {
        if (textField != null)
        {
            text = textField.value;
            Remove(textField);
            AddToClassList("editable-label");
            textField = null;
        }
    }

    private void OnLabelChanged(ChangeEvent<string> evt)
    {
        evt.StopPropagation();
    }
}