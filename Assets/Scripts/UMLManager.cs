using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                    _instance.transform.parent = null;
                    DontDestroyOnLoad(_instance);
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
                    CreateNewTree("First Tree");
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
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        treesDict = new Dictionary<long, UMLTree>();
    }

    private void Start()
    {
        //LoadTrees();
        foreach (UMLTree t in FindObjectsByType<UMLTree>(FindObjectsSortMode.InstanceID).ToList())
        {
            if(AddTreeToDict(t))
                CreateTreeButtonForTree(t);
        }
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

        if (!AddTreeToDict(newTree))
        {
            Destroy(newTree.gameObject);
            return;
        }

        newTree.TreeName = TreeName;
        CurrentTree = newTree;
        CreateTreeButtonForTree(newTree);
    }

    private void CreateTreeButtonForTree(UMLTree newTree)
    {
        UMLTreeButton button = Instantiate(TreeButtonPrefab, TreeList.transform);
        button.SetTree(newTree);
    }

    private bool AddTreeToDict(UMLTree tree)
    {
        if (treesDict.ContainsKey(tree.ID))
        {
            Debug.LogWarning("TreeID already exists: " + tree.ID);
            return false;
        }

        Debug.LogWarning("Tree added: " + tree.ID);
        treesDict.Add(tree.ID, tree);

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
}
