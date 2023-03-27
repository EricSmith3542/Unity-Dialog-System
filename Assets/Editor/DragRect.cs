using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DragRect : GraphElement
{

    public DragRect()
    {
        style.visibility = Visibility.Hidden;
        style.position = Position.Absolute;
        style.borderBottomWidth = 1;
        style.borderLeftWidth = 1;
        style.borderRightWidth = 1;
        style.borderTopWidth = 1;
        style.borderBottomColor = Color.white;
        style.borderLeftColor = Color.white;
        style.borderRightColor = Color.white;
        style.borderTopColor = Color.white;
    }
}
