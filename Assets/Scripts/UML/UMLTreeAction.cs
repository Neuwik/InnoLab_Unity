using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMLTreeAction : AUMLElement
{
    public UMLTree Tree;

    public override string Name { get { return Tree.TreeName + " Action"; } }
}
