using System.Collections;
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

            if (_instance != value)
            {
                if (_instance != null)
                {
                    Destroy(value.gameObject);
                }

                else
                {
                    _instance = value;
                    //_instance.transform.parent = null;
                    //DontDestroyOnLoad(_instance);
                }
            }
        }
    }

    public UMLTree TreePrefab;
    public UMLTreeButton TreeButtonPrefab;

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
                }
                else
                {
                    if (FindAndAddAllTreesOfScene() > 0)
                    {
                        _currentTree = treesDict.First().Value;
                    }
                    else
                    {
                        CreateNewTree("First Tree");
                    }
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
                _currentTree?.gameObject.SetActive(false);
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
        //LoadTrees();
        if (treesDict.Count == 0)
        {
            FindAndAddAllTreesOfScene();
        }
    }

    private int FindAndAddAllTreesOfScene()
    {
        List<UMLTree> trees = FindObjectsByType<UMLTree>(FindObjectsSortMode.InstanceID).ToList();
        foreach (UMLTree t in trees)
        {
            if (AddTreeToDict(t))
            {
                CreateTreeButtonForTree(t);
            }
        }
        return trees.Count;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            //SaveTrees();
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

        Debug.Log("Tree added: " + tree.ID);
        treesDict.Add(tree.ID, tree);

        OnTreesChanged.Invoke(trees);

        return true;
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
}
