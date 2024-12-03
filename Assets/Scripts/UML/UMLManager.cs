using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UMLManager : MonoBehaviour
{
    private static UMLManager _instance;
    public static UMLManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UML Manager is NULL");
            }
            return _instance;
        }
        set
        {
            if (value == null)
            {
                return;
            }

            if (_instance != null)
            {
                Destroy(value.gameObject);
            }

            if (_instance != value)
            {
                _instance = value;
                //_instance.transform.parent = null;
                //DontDestroyOnLoad(_instance);
            }
        }
    }

    public UMLTree TreePrefab;
    public UMLTreeButton TreeButtonPrefab;
    public List<UMLElementTypePrefab> ElementTypePrefabs;

    public GameObject BuildArea { get { return GameManager.Instance.UMLWindow.BuildArea; } }
    public GameObject TreeList { get { return GameManager.Instance.UMLWindow.TreeList; } }

    private Dictionary<long, UMLTree> treesDict;
    private List<UMLTree> trees
    {
        get { return treesDict.Values.ToList(); }
    }

    private UMLTree _currentTree;
    public UMLTree CurrentTree
    {
        get
        {
            if (_currentTree == null)
            {
                if (treesDict.Count > 0)
                {
                    _currentTree = treesDict.First().Value;
                    _currentTree.gameObject.SetActive(true);
                    OnCurrentTreeChanged.Invoke(_currentTree);
                }
                else
                {
                    Debug.LogWarning("Trees not loaded yet.");
                    return null;
                    /*
                    // Causes Bug because the trees are reloaded twice at the same time (on startup)
                    if (ReloadAllTrees() > 0)
                    {
                        _currentTree = treesDict.First().Value;
                    }
                    else
                    {
                        CreateNewTree("First Tree");
                    }
                    */
                }
            }
            return _currentTree;
        }
        private set
        {
            if (_currentTree == value)
            {
                return;
            }

            if (value != null)
            {
                _currentTree?.gameObject?.SetActive(false);
                _currentTree = value;
                _currentTree.gameObject.SetActive(true);
                OnCurrentTreeChanged.Invoke(_currentTree);
            }
        }
    }

    public UnityEvent<UMLTree> OnCurrentTreeChanged;
    public UnityEvent<List<UMLTree>> OnTreesChanged;

    private void Awake()
    {
        Instance = this;
        treesDict = new Dictionary<long, UMLTree>();

        OnTreesChanged.Invoke(trees);
    }

    private void Start()
    {
        ReloadAllTrees();
    }

    private int FindAndAddAllTreesOfScene()
    {
        List<UMLTree> sceneTrees = FindObjectsByType<UMLTree>(FindObjectsSortMode.InstanceID).ToList();
        foreach (UMLTree t in sceneTrees)
        {
            if (!AddTreeToDict(t))
            {
                Destroy(t);
            }
        }
        return sceneTrees.Count;
    }

    private void OnDestroy()
    {
        Debug.LogWarning("Destroying UMLManager");
        if (_instance == this)
        {
            SaveTrees();
        }
    }

    public void CreateNewTree(string TreeName = "New Tree")
    {
        UMLTree newTree = Instantiate(TreePrefab, BuildArea.transform);
        newTree.TreeName = TreeName;

        if (!AddTreeToDict(newTree))
        {
            Destroy(newTree.gameObject);
            return;
        }

        CreateTreeButtonForTree(newTree).Highlight();
        CurrentTree = newTree;
    }

    private UMLTreeButton CreateTreeButtonForTree(UMLTree newTree)
    {
        UMLTreeButton button = Instantiate(TreeButtonPrefab, TreeList.transform);
        button.SetTree(newTree);
        return button;
    }

    private bool AddTreeToDict(UMLTree tree)
    {
        if (treesDict.ContainsKey(tree.ID))
        {
            Debug.LogWarning("TreeID already exists: " + tree.ID);
            return false;
        }

        Debug.Log($"Tree added: {tree.UTreeName}");
        treesDict.Add(tree.ID, tree);

        UMLTreeButton btn = CreateTreeButtonForTree(tree);
        if (tree.gameObject.activeInHierarchy)
        {
            CurrentTree = tree;
            btn.Highlight();
        }

        OnTreesChanged.Invoke(trees);

        return true;
    }

    private UMLTree AddTreeToDict(UMLTreeData treeData)
    {
        if (treesDict.ContainsKey(treeData.ID))
        {
            Debug.LogWarning("TreeID already exists: " + treeData.ID);
            return null;
        }

        UMLTree tree = Instantiate(TreePrefab, BuildArea.transform);

        tree.ApplySimpleData(treeData);

        if (!AddTreeToDict(tree))
        {
            Destroy(tree.gameObject);
            return null;
        }
        /*
        CreateTreeButtonForTree(tree).Highlight();
        CurrentTree = tree;
        */
        OnTreesChanged.Invoke(trees);

        return tree;
    }

    public bool SetTreeAsCurrentTree(UMLTree tree)
    {
        if (tree == null)
        {
            return false;
        }
        if (!trees.Contains(tree))
        {
            return false;
        }

        CurrentTree = tree;

        return true;
    }

    public List<UMLTree> GetTreesAndSubscribeToTreesChanged(UnityAction<List<UMLTree>> listener)
    {
        OnTreesChanged.AddListener(listener);
        return trees;
    }

    public UMLTree GetTree(long id)
    {
        if (!treesDict.ContainsKey(id))
        {
            return null;
        }

        return treesDict[id];
    }

    public void RemoveTree(UMLTree tree)
    {
        if (tree != null)
        {
            if (treesDict.Remove(tree.ID))
            {
                if (tree.ID == CurrentTree.ID)
                {
                    CurrentTree = trees.FirstOrDefault();
                }

                Destroy(tree.gameObject);

                OnTreesChanged.Invoke(trees);
            }
        }
    }

    private void SaveTrees()
    {
        if (treesDict == null)
        {
            Debug.LogWarning("UML Manager: No trees found");
            return;
        }

        UMLSaveSystem.DeleteSaves();

        foreach (UMLTree tree in trees)
        {
            Debug.Log("UML Manager: Saving Tree " + tree.UTreeName);
            UMLSaveSystem.SaveTree(tree);
        }
    }

    private int ReloadAllTrees()
    {
        treesDict = new Dictionary<long, UMLTree>();

        int count = 0;

        count += FindAndAddAllTreesOfScene();
        count += LoadTreesFrommSave();

        OnTreesChanged.Invoke(trees);

        return count;
    }

    private int LoadTreesFrommSave()
    {
        List<UMLTreeData> treesData = UMLSaveSystem.LoadTrees();
        Dictionary<UMLTreeData, UMLTree> newTrees = new Dictionary<UMLTreeData, UMLTree>();

        foreach (UMLTreeData treeData in treesData)
        {
            UMLTree newTree = AddTreeToDict(treeData);
            if (newTree != null)
            {
                newTrees.Add(treeData, newTree);
            }
        }

        foreach (var item in newTrees)
        {
            if (!item.Value.ApplyElementData(item.Key))
            {
                Debug.LogWarning("UML Manager: Could not load Tree " + item.Key.ID + " - " + item.Key.TreeName);
                RemoveTree(item.Value);
            }
        }

        return newTrees.Count;
    }
}
