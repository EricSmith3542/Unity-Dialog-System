using UnityEditor;
using UnityEngine.UIElements;

public class EditableLabel : Label
{
    private TextField textField;
    private string oldText;

    public EditableLabel(string labelText) : base(labelText)
    {
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<ChangeEvent<string>>(OnLabelChanged);
        style.alignSelf = Align.Center;
    }
    public EditableLabel() : this("") { }

    private void OnMouseDown(MouseDownEvent evt)
    {
        if (evt.clickCount == 2 && textField == null)
        {
            oldText = text;
            textField = new TextField { value = text };
            textField.RegisterCallback<BlurEvent>(OnTextFieldBlur);
            Add(textField);
            RemoveFromClassList("editable-label");
            textField.SelectAll();
            evt.StopPropagation();
        }
    }

    //TODO: IMPROVE EDITABLE LABE
    //Need to ensure Option name uniqueness within nodes
    //Make it smoother to use
    private void OnTextFieldBlur(BlurEvent evt)
    {
        if (textField != null)
        {
            string newName = textField.value;
            //Dialog and Boolean nodes handle output name uniqueness differently
            //so we switch depending on the parent type and ensure the new text of the string obeys uniqueness rules
            DialogTreeNode parentNode = GetFirstAncestorOfType<DialogTreeNode>();
            bool validName = false;
            switch (parentNode)
            {
                case DialogNode dialogNode:
                    validName = dialogNode.IsUniqueOutputName(newName);
                    break;
                case BooleanNode booleanNode:
                    validName = booleanNode.IsUniqueOutputName(newName);
                    break;
                default:
                    break;
            }

            if (validName || newName == oldText)
            {
                text = newName;
                Remove(textField);
                AddToClassList("editable-label");
                parentNode.ChangeName(oldText, newName);
                textField = null;
                oldText = null;
            }
            else
            {
                EditorUtility.DisplayDialog("Invalid Port Name", "The port name \"" + newName + "\" violates the uniqueness rules of this node type. Please choose a different name.", "OK");
            }
            
        }
    }

    private void OnLabelChanged(ChangeEvent<string> evt)
    {
        evt.StopPropagation();
    }

    public static EditableLabel FetchEditableLabel(VisualElement element)
    {
        return element.Query<EditableLabel>().First();
    }
}