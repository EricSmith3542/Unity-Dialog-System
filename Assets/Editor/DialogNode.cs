using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNode : DialogTreeNode
{
    string dialog = "";
    public string Dialog { get => dialog; set => dialog = value; }

    int numOutputs = 0;

    public DialogNode(string title, DialogTreeGraphView gv, Rect pos) : base(title, gv, pos)
    {
        BuildRequirementsPort();
        BuildPortAddSubtractButtons();
        AddOutputPort();
        BuildTextArea();
    }
    public DialogNode(string title, DialogTreeGraphView gv) : this(title, gv, new Rect(0, 0, 0, 0)) { }
    public DialogNode(DialogTreeGraphView gv) : this("Start", gv) { }
        
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        // Add a menu item that performs some action
        evt.menu.AppendAction("Do Something", action => DoSomething(), DropdownMenuAction.AlwaysEnabled);
    }

    private void DoSomething()
    {
        // Perform some action
    }

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

    private void AddOutputPort()
    {
        Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        output.AddManipulator(new EdgeDragHelper(graphView));
        output.portName = "Option " + numOutputs++;
        output.portColor = Color.red;
        outputContainer.Add(output);
    }
    private void RemoveOutputPort()
    {
        outputContainer.RemoveAt(outputContainer.childCount - 1);
        numOutputs--;
    }

    private void BuildTextArea()
    {
        var foldout = new Foldout();
        foldout.text = "Dialogue Text";
        mainContainer.Add(foldout);

        var textField = new TextField();
        textField.multiline = true;
        textField.RegisterValueChangedCallback(evt =>
        {
            Dialog = evt.newValue;
        });

        //Nest text area in scrollview and foldout
        ScrollView scroll = new ScrollView();
        scroll.Add(textField);
        foldout.Add(scroll);
    }
}
