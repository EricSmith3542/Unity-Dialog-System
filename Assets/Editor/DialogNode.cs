using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNode : DialogTreeNode
{
    private string dialog = "";
    public string Dialog { get => dialog; set => SetDialog(value); }
    private TextField dialogTextField;

    public DialogNode(string title, DialogTreeGraphView gv, string id, Rect pos) : base(title, gv, id, pos)
    {
        defaultPortPrefix = "Option ";
        BuildRequirementsPort();
        BuildPortAddSubtractButtons();
        AddOutputPort();
        BuildTextArea();
    }
    public DialogNode(string title, DialogTreeGraphView gv, string id) : this(title, gv, id, new Rect(0, 0, 0, 0)) { }
    public DialogNode(DialogTreeGraphView gv, string id) : this("Start", gv, id) { }

    public override NodeData AsData()
    {
        SerializeableMap connections = new SerializeableMap();
        foreach (Port outputPort in outputContainer.Query<Port>().ToList())
        {
            List<string> inputIds = new List<string>();
            foreach (Edge edge in outputPort.connections)
            {
                inputIds.Add((edge.input.GetFirstAncestorOfType<DialogTreeNode>()).id);
            }
            connections.Add(EditableLabel.FetchEditableLabel(outputPort).text, inputIds);
        }
        return new DialogNodeData(id, nodeTitle, GetPosition(), connections, Dialog);
    }

    //CONTEXT MENU EXAMPLE
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        // Add a menu item that performs some action
        evt.menu.AppendAction("Do Something", action => DoSomething(), DropdownMenuAction.AlwaysEnabled);
    }

    private void DoSomething()
    {
        // Perform some action
    }
    //END CONTEXT MENU EXAMPLE

    private void BuildRequirementsPort()
    {
        Port input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        input.AddManipulator(new EdgeDragHelper(graphView));
        input.portName = "Requirements";
        input.portColor = Color.green;
        inputContainer.Add(input);
    }

    private void BuildPortAddSubtractButtons()
    {
        var addButton = new Button(AddOutputPort);
        addButton.text = "+";
        addButton.tooltip = "Add Response Option";
        outputContainer.Add(addButton);

        var subButton = new Button(RemoveOutputPort);
        subButton.text = "-";
        subButton.tooltip = "Remove Last Response Option";
        outputContainer.Add(subButton);
    }

    
    private void RemoveOutputPort()
    {
        outputContainer.RemoveAt(outputContainer.childCount - 1);
        portCount--;
    }

    private void BuildTextArea()
    {
        Foldout foldout = new Foldout();
        foldout.text = "Dialogue Text";
        mainContainer.Add(foldout);

        TextField textField = new TextField();
        textField.multiline = true;
        textField.RegisterValueChangedCallback(evt =>
        {
            Dialog = evt.newValue;
        });
        dialogTextField = textField;

        //Nest text area in scrollview and foldout
        ScrollView scroll = new ScrollView();
        scroll.Add(textField);
        foldout.Add(scroll);
    }

    private void SetDialog(string text)
    {
        dialog = text;
        dialogTextField.value = text;
    }
}
