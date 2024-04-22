using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        Player = GameObject.FindWithTag("Player");
        PlayerGarbageCollector = Player.GetComponent<GarbageCollector>();
        UMLPanel = GameObject.FindWithTag("UMLPanel");


        UMLActors = FindObjectsByType<UMLActor>(FindObjectsSortMode.InstanceID).ToList();
        Garbages = FindObjectsByType<Garbage>(FindObjectsSortMode.InstanceID).ToList();
    }

    public GameObject Player { get; private set; }
    public GarbageCollector PlayerGarbageCollector { get; private set; }
    public GameObject UMLPanel { get; private set; }

    //For Reset
    public List<Garbage> Garbages;

    //UML Testing
    public List<UMLActor> UMLActors;
    public Button UMLStart;
    public Button UMLStop;

    public void RunUML()
    {
        UMLActors.ForEach(a => StartCoroutine(a.StartUML()));
        StartCoroutine(AllBotsDone());
    }

    private void StopUML()
    {
        UMLActors.ForEach(a => a.StopUML());
    }

    public IEnumerator AllBotsDone()
    {
        yield return new WaitUntil(() => (UMLActors.Find(a => a.UMLRunning) == null)); //Not Performant?
        ShowWinLose();
        ResetLevel();
        UMLStop.gameObject.SetActive(false);
        UMLStart.gameObject.SetActive(true);
        yield break;
    }

    private void ResetLevel()
    {
        UMLActors.ForEach(a => a.Reset());
        Garbages?.ForEach(g => g.Reset());
    }

    public void ShowWinLose()
    {
        Debug.Log("Game Won/Lost");
    }
}
