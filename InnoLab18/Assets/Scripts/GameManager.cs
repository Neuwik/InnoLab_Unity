using System.Collections;
using System.Collections.Generic;
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
    }

    public GameObject Player { get; private set; }
    public GarbageCollector PlayerGarbageCollector { get; private set; }
    public GameObject UMLPanel { get; private set; }


    //UML Testing
    public UMLActor UMLActor;
    public Button UMLStart;
    public Button UMLStop;
    public void RunUML()
    {
        UMLActor?.StartUML();
        StartCoroutine(AllBotsDone());
    }

    public void StopUML()
    {
        StopCoroutine(AllBotsDone());
        UMLActor?.StopUML();
    }

    public IEnumerator AllBotsDone()
    {
        yield return new WaitUntil(() => (!UMLActor?.UMLRunning) ?? true);
        ShowWinLoose();
        UMLActor?.ResetActor();
        UMLStop.gameObject.SetActive(false);
        UMLStart.gameObject.SetActive(true);
        yield break;
    }

    public void ShowWinLoose()
    {
        Debug.Log("Game Won/Lost");
    }
}
