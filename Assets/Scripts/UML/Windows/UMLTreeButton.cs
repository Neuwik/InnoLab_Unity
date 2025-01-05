using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UMLTreeButton : MonoBehaviour
{
    private UMLTree tree;

    [SerializeField]
    private TMP_InputField text;

    [SerializeField]
    private Button renameButton;

    [SerializeField]
    private Color highlightColor = Color.red;
    private Color baseColor;

    [SerializeField]
    private Image image;

    private static UMLTreeButton currentlyHighlightedButton;

    private void Awake()
    {
        text.interactable = false;
        baseColor = image.color;

        text.onEndEdit.AddListener(OnNameEditEnd);

        renameButton.onClick.AddListener(OnRenameButtonClick);
    }

    private void OnDestroy()
    {
        if (tree != null)
        {
            tree.OnDestroyEvent.RemoveListener(OnDestroy);
        }
    }

    public void SetTree(UMLTree tree)
    {
        this.tree = tree;
        text.text = tree.TreeName;
        name = "btn_" + tree.UTreeName;

        if (tree != null)
        {
            tree.OnDestroyEvent.AddListener(() => Destroy(gameObject));
        }
    }

    public void OnButtonClick()
{
    if (tree == null)
    {
        Destroy(gameObject);
    }
    else
    {
        if (Input.GetMouseButtonDown(2))
        {
            // NOT WORKING ---> TODO
            // Middle Mouse Button -> delete
            UMLManager.Instance.RemoveTree(tree); 
            Destroy(gameObject); 
        }
        else
        {
            SelectTree();
        }
    }
}

    private void SelectTree()
    {
        if (tree == null) return;

        if (currentlyHighlightedButton != null && currentlyHighlightedButton != this)
        {
            currentlyHighlightedButton.StopHighlight();
        }

        Highlight();
        currentlyHighlightedButton = this;

        UMLManager.Instance.SetTreeAsCurrentTree(tree);
    }

    private void Highlight()
    {
        image.color = highlightColor;
    }

    private void StopHighlight()
    {
        image.color = baseColor;
    }

    private void OnNameEditEnd(string newName)
    {
        if (string.IsNullOrEmpty(newName))
        {
            text.text = tree.TreeName;
        }
        else
        {
            UMLManager.Instance.RenameTree(tree, newName);
            tree.TreeName = newName;
        }

        text.interactable = false;
    }

    private void OnRenameButtonClick()
    {
        text.interactable = true;
        text.ActivateInputField();
    }
}
