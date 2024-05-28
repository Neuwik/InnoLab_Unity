using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manger is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);

        UMLActors = FindObjectsByType<UMLActor>(FindObjectsSortMode.InstanceID).ToList();
        Enemies = FindObjectsByType<EnemyMovementController>(FindObjectsSortMode.InstanceID).ToList();
        ResetableComponents = new List<IResetable>();
        ReDrawArrow = false;

        if (Console == null)
        {
            Console = FindObjectOfType<ConsoleManager>();
        }

        if (TickManager == null)
        {
            TickManager = GetComponent<TickManager>();
        }
    }

    public ConsoleManager Console;

    //For Reset
    [HideInInspector]
    public List<IResetable> ResetableComponents;

    //UML Testing
    [HideInInspector]
    public List<UMLActor> UMLActors;
    [HideInInspector]
    public List<EnemyMovementController> Enemies;
    public Button UMLStart;
    public Button UMLStop;
    public TickManager TickManager;
    public bool UMLIsRunning = false;
    public UMLTree CurrentTree;

    //UML Objects
    private GameObject _uml_canvas;
    public GameObject UML_Canvas
    {
        get
        {
            if (_uml_canvas == null)
            {
                _uml_canvas = GameObject.FindGameObjectWithTag("UMLCanvas");
            }
            return _uml_canvas;
        }
    }
    private GameObject _uml_selectionPanel;
    public GameObject UML_SelectionPanel
    {
        get
        {
            if (_uml_selectionPanel == null)
            {
                _uml_selectionPanel = GameObject.FindGameObjectWithTag("UMLSelectionPanel");
            }
            return _uml_selectionPanel;
        }
    }
    private GameObject _uml_panel;
    public GameObject UML_Panel
    {
        get
        {
            if (_uml_panel == null)
            {
                _uml_panel = GameObject.FindGameObjectWithTag("UMLPanel");
            }
            return _uml_panel;
        }
    }

    //drawing arrows
    [HideInInspector]
    public GameObject ActiveArrow;
    [HideInInspector]
    public bool ReDrawArrow;

    public void RunUML()
    {
        UMLIsRunning = true;
        TickManager.StartTicks();
        UMLActors.ForEach(a => StartCoroutine(a.StartUML()));
        Enemies.ForEach(e => e.StartMovement());
        StartCoroutine(AllBotsDone());
    }

    public void StopUML()
    {
        UMLIsRunning = false;
        UMLActors.ForEach(a => a.Stop());
        TickManager.Reset();
    }

    public IEnumerator AllBotsDone()
    {
        yield return new WaitUntil(() => (UMLActors.Find(a => !a.UMLFinished) == null)); //Not Performant?
        ShowWinLose();
        ResetLevel();
        UMLStop.gameObject.SetActive(false);
        UMLStart.gameObject.SetActive(true);
        yield break;
    }

    private void ResetLevel()
    {
        //UMLActors.ForEach(a => a.Reset());
        UMLIsRunning = false;
        SearchForResetableComponents();
        TickManager.Reset();
        ResetableComponents?.ForEach(r => r.Reset());
    }

    private void SearchForResetableComponents()
    {
        // Not performant
        if (ResetableComponents == null)
        {
            ResetableComponents = new List<IResetable>();
        }
        else
        {
            ResetableComponents.Clear();
        }

        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            IResetable[] childrenInterfaces = rootGameObject.GetComponentsInChildren<IResetable>(includeInactive: true);
            foreach (var childInterface in childrenInterfaces)
            {
                ResetableComponents.Add(childInterface);
            }
        }
    }

    public void ShowWinLose()
    {
        Debug.Log("Game Won/Lost");
    }

}
