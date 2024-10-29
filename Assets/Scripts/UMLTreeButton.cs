using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UMLTreeButton : MonoBehaviour
{
    private UMLTree tree;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private Color highlightColor = Color.red;
    private Color baseColor;

    [SerializeField]
    private Image image;

    private bool isHighlighted = false;

    private void Awake()
    {
        baseColor = image.color;
        UMLManager.Instance.OnCurrentTreeChanged.AddListener(OnSelectedTreeChanged);
    }

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
            else
            {
                Highlight();
            }
        }
    }

    private void OnSelectedTreeChanged(UMLTree selectedTree)
    {
        if (isHighlighted && selectedTree.ID != tree.ID)
        {
            StopHighlight();
        }
    }

    public void Highlight()
    {
        isHighlighted = true;
        image.color = highlightColor;
    }

    private void StopHighlight()
    {
        isHighlighted = false;
        image.color = baseColor;
    }
}
