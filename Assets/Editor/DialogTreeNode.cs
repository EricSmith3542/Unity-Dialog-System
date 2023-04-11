using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

//This class could use a better name
public abstract class DialogTreeNode : Node
{
    protected static DialogTreeGraphView graphView;
    protected string defaultPortPrefix;
    protected int portCount;
    public string nodeTitle;
    public string id;

    protected DialogTreeNode(string title, string id, Rect pos)
    {
        this.id = id;
        nodeTitle = title;
        SetPosition(pos);
        SetNodeCapabilites();

        //Replace the boring titleLabel with an EditableLabel
        titleContainer.Remove(this.Query<Label>("title-label").First());
        var editableLabel = new EditableLabel(nodeTitle);
        editableLabel.RegisterValueChangedCallback(evt =>
        {
            nodeTitle = evt.newValue;
        });
        titleContainer.Insert(0, editableLabel);

        this.AddManipulator(new NodeDragger(graphView));
        this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
    }
    protected void SetNodeCapabilites()
    {
        capabilities |= Capabilities.Movable |
                                Capabilities.Resizable |
                                Capabilities.Deletable |
                                Capabilities.Selectable |
                                Capabilities.Copiable |
                                Capabilities.Ascendable |
                                Capabilities.Collapsible |
                                Capabilities.Droppable |
                                Capabilities.Renamable |
                                Capabilities.Snappable |
                                Capabilities.Groupable |
                                Capabilities.Stackable;
    }
    public static void SetGraphView(DialogTreeGraphView gv)
    {
        graphView = gv;
    }

    protected void AddOutputPort()
    {
        AddOutputPort(defaultPortPrefix + portCount++);
    }
    public virtual void AddOutputPort(string portName)
    {
        Port output = InstantiateEditablePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        output.AddManipulator(new EdgeDragHelper(graphView));
        EditableLabel.FetchEditableLabel(output).text = portName;
        output.portColor = Color.red;
        outputContainer.Add(output);
    }

    public Port InstantiateEditablePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
    {
        Port port = base.InstantiatePort(orientation, direction, capacity, type);

        //For some reason, removing this label breaks all the functionality of the Port :)
        Label portLabel = port.Query<Label>().First();
        portLabel.text = "";

        EditableLabel newLabel = new EditableLabel();
        port.Add(newLabel);

        return port;
    }

    public abstract NodeData AsData();
    public abstract bool IsUniqueOutputName(string name);

    //Method for updating the uniqueness tracker of sub-classes when ports are renamed
    public abstract void ChangeName(string oldName, string newName);

    public static void DisplayVisualElementHierarchy(VisualElement element, int depth = 0)
    {
        switch (depth)
        {
            case 0:
                element.style.backgroundColor = Color.black;
                break;
            case 1:
                element.style.backgroundColor = Color.gray;
                break;
            case 2:
                element.style.backgroundColor = Color.white;
                break;
            case 3:
                element.style.backgroundColor = Color.cyan;
                break;
            default:
                break;
        }
        element.style.borderBottomColor = Color.red;
        element.style.borderLeftColor = Color.red;
        element.style.borderRightColor = Color.red;
        element.style.borderTopColor = Color.red;

        element.style.borderBottomWidth = 1f;
        element.style.borderLeftWidth = 1f;
        element.style.borderRightWidth = 1f;
        element.style.borderTopWidth = 1f;
        // Display information about this element
        Debug.Log(new string('-', depth) + " " + element.GetType() + " color: " + element.style.backgroundColor);
        
        // Recursively display information about each child element
        foreach (VisualElement child in element.Children())
        {
            DisplayVisualElementHierarchy(child, depth + 1);
        }
    }
}
