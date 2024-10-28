using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UMLTreeButton : MonoBehaviour
{
    private UMLTree tree;

    [SerializeField]
    private TMP_Text text;

    public void SetTree(UMLTree tree)
    {
        this.tree = tree;
        text.text = tree.TreeName;
        name = "btn_"+tree.UTreeName;
    }

    public void OnButtonClick()
    {
        if (tree == null)
        {
            Destroy(gameObject);
        }
        else
        {
            if (!UMLManager.Instance.SetTreeAsCurrentTree(tree))
            {
                // Tree is not in UML Manager
                Destroy(tree.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
