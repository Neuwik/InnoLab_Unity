using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMLTreeStart : AUMLElement
{
    [SerializeField]
    private UMLTree tree;

    public override string Name { get { return tree.TreeName + " Start"; } }
}
