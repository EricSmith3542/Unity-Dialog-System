﻿using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNode : DialogTreeNode
{
    private const string TYPE_ID = "DIALOG";
    private ColorField borderColorField, backGroundColorField;
    private ObjectField borderSpriteField, backGroundSpriteField;
    private TextField dialogTextField, displayNameField;
    private HashSet<string> outputNameSet;
    private string dialog = "";

    public string Dialog { get => dialog; set => SetDialog(value); }
    public string DisplayName 
    { 
        get => displayNameField.value; 
        set => displayNameField.value = value;  
    }
    public Color BorderColor
    {
        get => borderColorField.value;
        set => borderColorField.value = value;
    }
    public Color BackgroundColor
    {
        get => backGroundColorField.value;
        set => backGroundColorField.value = value;
    }
    public Sprite BorderSprite
    {
        get => (Sprite)borderSpriteField.value;
        set => borderSpriteField.value = value;
    }
    public Sprite BackgroundSprite
    {
        get => (Sprite)backGroundSpriteField.value;
        set => backGroundSpriteField.value = value;
    }
    

    public DialogNode(string title, string id, Rect pos) : base(title, id, pos)
    {
        outputNameSet = new HashSet<string>();
        defaultPortPrefix = "Option ";
        BuildRequirementsPort();
        BuildInputFields();
        BuildPortAddSubtractButtons();
        AddOutputPort();
        BuildTextArea();
    }
    public DialogNode(string title, string id) : this(title, id, new Rect(0, 0, 0, 0)) { }
    public DialogNode(string id) : this("Start", id) { }

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
        return new DialogNodeData(TYPE_ID, id, nodeTitle, GetPosition(), DisplayName, Dialog, BorderColor, BorderSprite, BackgroundColor, BackgroundSprite, connections);
    }

    public Color GetBorderColor() { return borderColorField.value; }
    public Color GetBackgroundColor() { return backGroundColorField.value; }
    public Sprite GetBorderSprite() { return (Sprite)borderSpriteField.value; }
    public Sprite GetBackgroundSprite() { return (Sprite)backGroundSpriteField.value; }
    public void SetBorderColor(Color borderColor) { borderColorField.value = borderColor; }
    public void SetBorderSprite(Sprite borderSprite) { borderSpriteField.value = borderSprite; }
    public void SetBackgroundColor(Color backGroundColor) { backGroundColorField.value = backGroundColor; }
    public void SetBackgroundSprite(Sprite backGroundSprite) { backGroundSpriteField.value = backGroundSprite; }

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

    public override void AddOutputPort(string portName)
    {
        if (!outputNameSet.Contains(portName))
        {
            base.AddOutputPort(portName);
            outputNameSet.Add(portName);
        }
        else
        {
            Debug.LogError("Tried to name the new port \"" + portName + "\" when a port with that name already exists on this node. Port names must be unique within a node.");
        }
        
    }

    public override bool IsUniqueOutputName(string name)
    {
        return !outputNameSet.Contains(name);
    }

    public override void ChangeName(string oldName, string newName)
    {
        if (oldName == newName) { return; }
        outputNameSet.Remove(oldName);
        outputNameSet.Add(newName);
    }

    private void BuildRequirementsPort()
    {
        Port input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        input.AddManipulator(new EdgeDragHelper(graphView));
        input.portName = "Requirements";
        input.portColor = Color.green;
        inputContainer.Add(input);
        AddDivider(inputContainer);
    }

    private void AddDivider(VisualElement parentElement)
    {
        VisualElement divider = new VisualElement();
        divider.AddToClassList("divider");
        divider.style.height = 1;
        divider.style.backgroundColor = new Color(.1f, .1f, .1f);
        parentElement.Add(divider);
    }

    private void BuildInputFields()
    {
        displayNameField = new TextField("Display Name") { value = "???" };
        borderColorField = new ColorField("Border Color/Tint");
        borderSpriteField = new ObjectField("Border Sprite") { objectType = typeof(Sprite) };
        backGroundColorField = new ColorField("Background Color/Tint");
        backGroundSpriteField = new ObjectField("Background Sprite") { objectType = typeof(Sprite) }; ;

        inputContainer.Add(displayNameField);
        inputContainer.Add(borderColorField);
        inputContainer.Add(borderSpriteField);
        inputContainer.Add(backGroundColorField);
        inputContainer.Add(backGroundSpriteField);
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

    
    public void RemoveOutputPort()
    {
        VisualElement removePort = outputContainer.ElementAt(outputContainer.childCount - 1);
        outputNameSet.Remove(EditableLabel.FetchEditableLabel(removePort).text);
        outputContainer.Remove(removePort);
        portCount--;
    }

    private void BuildTextArea()
    {
        AddDivider(mainContainer);

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
