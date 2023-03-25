using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTreeNode
{
    List<DialogTreeNode> requirements;
    List<DialogTreeNode> requirementFor;

    //TODO - Instead of a list of strings, this will be a list of DialogEvent objects which will contain Dialog Box configurations to change images, text styles/fonts/speeds, etc
    List<string> lines;
    public List<string> Lines
    {
        get { return lines; }
        set { lines = value; }
    }
}
