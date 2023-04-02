using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogTreeNode : Node
{
    protected DialogTreeGraphView graphView;
    protected string id;

    protected DialogTreeNode(string title, DialogTreeGraphView gv, string id, Rect pos)
    {
        this.id = id;
        graphView = gv;
        SetPosition(pos);
        SetNodeCapabilites();

        //Replace the boring titleLabel with an EditableLabel
        titleContainer.Remove(this.Query<Label>("title-label").First());
        var editableLabel = new EditableLabel(title);
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
}
