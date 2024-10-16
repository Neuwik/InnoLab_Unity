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
        if (_instance != null)
        {
            //Debug.LogWarning("Game Manger exists " + _instance.name);
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            transform.parent = null;
            DontDestroyOnLoad(this);
            //LoadReferences();
            SceneManager.sceneLoaded += OnSceneLoad;
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.LogWarning("Game Manager: New Scene loaded");
        LoadReferences();
    }

    private void LoadReferences()
    {
        UMLActors = FindObjectsByType<UMLActor>(FindObjectsSortMode.InstanceID).ToList();
        Enemies = FindObjectsByType<EnemyMovementController>(FindObjectsSortMode.InstanceID).ToList();
        Garbages = FindObjectsByType<Garbage>(FindObjectsSortMode.InstanceID).ToList();
        GarbageCollectors = FindObjectsByType<GarbageCollector>(FindObjectsSortMode.InstanceID).ToList();
        ResetableComponents = new List<IResetable>();
        ReDrawArrow = false;

        if (Console == null)
        {
            Console = FindObjectOfType<ConsoleManager>();
        }

        if (TickManager == null)
        {
            TickManager = FindObjectOfType<TickManager>();
        }

        if (CurrentTree == null)
        {
            CurrentTree = FindObjectOfType<UMLTree>();
        }

        if (LevelOutcome == null)
        {
            LevelOutcome = FindObjectOfType<LevelOutcome>(true);
        }

        if (LevelProgress == null)
        {
            LevelProgress = FindObjectOfType<LevelProgress>();
        }

        if (LevelManager == null)
        {
            LevelManager = FindObjectOfType<LevelManager>();
        }

        if(btn_UMLStart == null || btn_UMLStop == null)
        {
            UMLStartStop startstop = FindObjectOfType<UMLStartStop>();
            btn_UMLStart = startstop?.btn_Start;
            btn_UMLStop = startstop?.btn_Stop;
        }
        btn_UMLStart?.onClick.AddListener(RunUML);
        btn_UMLStop?.onClick.AddListener(StopUML);
    }

    public ConsoleManager Console;

    //For Reset
    [HideInInspector]
    public List<IResetable> ResetableComponents;

    [HideInInspector]
    public List<UMLActor> UMLActors;
    [HideInInspector]
    public List<EnemyMovementController> Enemies;
    [HideInInspector]
    public List<Garbage> Garbages;
    [HideInInspector]
    public List<GarbageCollector> GarbageCollectors;
    public Button btn_UMLStart;
    public Button btn_UMLStop;
    public TickManager TickManager;
    public bool UMLIsRunning = false;
    public UMLTree CurrentTree;
    public LevelOutcome LevelOutcome;
    public LevelProgress LevelProgress;
    public LevelManager LevelManager;

    // For Level Outcome Tracking
    private StarCalculationValues outcomeCalculationValues;

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
    private GameObject _uml_content;
    public GameObject UML_Content
    {
        get
        {
            if (_uml_content == null)
            {
                _uml_content = GameObject.FindGameObjectWithTag("UMLContent");
            }
            return _uml_content;
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
        UMLActors.ForEach(a =>
        {
            if (a.Tree == null)
            {
                a.Tree = CurrentTree;
            }
            StartCoroutine(a.StartUML());
        });
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

        UMLIsRunning = false;

        UpdateLevelProgress();
        LevelOutcome.ShowLevelOutcome(outcomeCalculationValues);
        btn_UMLStart.enabled = false;
        btn_UMLStop.enabled = false;
        yield break;
    }

    public void ResetLevel()
    {
        //UMLActors.ForEach(a => a.Reset());
        btn_UMLStop.gameObject.SetActive(false);
        btn_UMLStart.gameObject.SetActive(true);
        btn_UMLStart.enabled = true;
        btn_UMLStop.enabled = true;
        UMLIsRunning = false;
        SearchForResetableComponents();
        TickManager.Reset();
        ResetableComponents?.ForEach(r => r.Reset());
        UpdateLevelProgress();
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

    public void UpdateLevelProgress()
    {
        int trashCount = 0;
        GarbageCollectors.ForEach(gc => trashCount += gc.GarbageCount);
        
        bool allAlife = !(UMLActors.Where(a => a.State != EUMLActorState.Done).ToList().Count > 0);

        float percentHealthSum = 0;
        float percentEnergySum = 0;

        UMLActors.ForEach(a =>
        {
            percentHealthSum += a.GetComponent<PlayerHealth>().PercentHealth;
            percentEnergySum += a.Battery.PercentEnergy;
        });

        outcomeCalculationValues = new StarCalculationValues
        {
            collectedTrash = trashCount,
            maxTrashCount = Garbages.Count,
            allAlife = allAlife,
            percentHealth = percentHealthSum / UMLActors.Count,
            percentEnergy = percentEnergySum / UMLActors.Count
        };

        Debug.Log(outcomeCalculationValues.percentHealth);
        Debug.Log(outcomeCalculationValues.percentEnergy);
        Debug.Log(outcomeCalculationValues.SuccessQualityPercent);

        LevelProgress.UpdateLevelProgress(outcomeCalculationValues);
    }
}
