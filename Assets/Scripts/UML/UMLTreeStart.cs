using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMLTreeStart : AUMLElement
{
    private UMLTree _tree;

    public override string Name { get { return _tree.name + " Start"; } }
}
