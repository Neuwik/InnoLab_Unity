using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
