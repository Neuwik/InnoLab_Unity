using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UMLTreeAction : AUMLElement, IResetable
{
    public UMLTree Tree;
    public override string Name { get { return Tree.TreeName + " Action"; } }

    [SerializeField]
    private TMP_Dropdown dropDown;

    private static Dictionary<long, string> selectableTrees;
    public static Dictionary<long, string> SelectableTrees
    {
        get
        {
            if (selectableTrees == null)
                UpdateSlectableTrees(UMLManager.Instance.GetTreesAndSubscribeToTreesChanged(UpdateSlectableTrees));

            return selectableTrees;
        }
    }

    private static UnityEvent OnSelectableTreesChanged = new UnityEvent();

    private void Awake()
    {
        dropDown.onValueChanged.AddListener(SelectedTreeChanged);
        OnSelectableTreesChanged.AddListener(UpdateDropDownOptions);
    }

    private new void Start()
    {
        base.Start();
        SelectableTrees.Count(); // Update at first call
    }

    public void Reset()
    {
        highlightCounter = 0;
        StopHighlight();
    }

    private void OnDestroy()
    {
        OnSelectableTreesChanged.RemoveListener(UpdateDropDownOptions);
    }

    private static void UpdateSlectableTrees(List<UMLTree> trees)
    {
        selectableTrees = new Dictionary<long, string>();
        foreach (UMLTree tree in trees)
        {
            selectableTrees.Add(tree.ID, tree.TreeName);
        }
        OnSelectableTreesChanged.Invoke();
    }

    private void UpdateDropDownOptions()
    {
        Debug.LogWarning("Tree Action: UpdateDropDownOptions");
        dropDown.options.Clear();
        foreach (var item in SelectableTrees)
        {
            dropDown.options.Add(new TMP_Dropdown.OptionData(item.Value));
            if (Tree != null && item.Key == Tree.ID)
            {
                // Might not trigger "SelectedTreeChanged" because Index could be the same and value setter returns without Invoking onValueChanged
                // dropDown.value = dropDown.options.Count - 1;
                int newIndex = dropDown.options.Count - 1;
                dropDown.SetValueWithoutNotify(newIndex);
                SelectedTreeChanged(newIndex);
            }
        }

        if (Tree == null && SelectableTrees.Count > 0)
        {
            // Does not trigger "SelectedTreeChanged" because Index stays the same and value setter returns without Invoking onValueChanged
            // dropDown.value = 0;

            dropDown.SetValueWithoutNotify(0);
            SelectedTreeChanged(0);
        }
    }

    private void SelectedTreeChanged(int index)
    {
        Debug.Log("Tree Action: SelectedTreeChanged");
        if (index < 0 || index >= SelectableTrees.Count)
        {
            Debug.LogWarning("Selected Tree (Tree Action Drop Down) does not exist");
            return;
        }
        Tree = UMLManager.Instance.GetTree(SelectableTrees.ElementAt(index).Key);
        dropDown.RefreshShownValue();
    }
}
