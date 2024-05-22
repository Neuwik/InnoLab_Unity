using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.Search;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro.EditorUtilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class LevelOutcome : MonoBehaviour
{
    #region DEBUG

    public bool playerDied = false;
    public bool garbageCollected = false;

    public bool condition1_IsTrue = true;
    public bool condition2_IsTrue = true;
    public bool condition3_IsTrue = true;

    #endregion

    public int forStar_maxSteps;
    public int forStar_maxUMLElements;

    public GameManager gameManager;

    public GameObject LevelOutcomePrefab;
    public GameObject Stars;
    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;

    public TMP_Text LevelIndexText;
    public TMP_Text OutcomeText;

    public Button NextBtn;
    public Button ResetBtn;

    private int starsEarnedAmount = 0;

    private bool _isLevelActive = true;
    private Vector2 _posNextLevelBtn;


    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
            gameManager = GameManager.Instance;

        if (LevelOutcomePrefab == null)
        {
            LevelOutcomePrefab = gameObject;
        }

        LevelOutcomePrefab.SetActive(false);

        _posNextLevelBtn = NextBtn.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLevelActive)
        {
            // Level success
            if(!playerDied)
            {
                if (garbageCollected)
                    EndLevel(true);
            }
            // Level failed
            else
            {
                EndLevel(false);
            }

            //Implement these counters
            /*
            if(gameManager.steps > forStar_maxSteps)
                condition2_IsTrue = false;

            if(gameManager.UMLElements.Count > forStar_maxUMLElements)
                condition3_IsTrue = false;
            */
        }
    }

    private void EndLevel(bool isSuccess)
    {
        _isLevelActive = false;
        LevelOutcomePrefab.SetActive(true);

        // Level success
        if (isSuccess)
        {
            if (condition1_IsTrue)
                starsEarnedAmount++;
            if (condition2_IsTrue)
                starsEarnedAmount++;
            if (condition3_IsTrue)
                starsEarnedAmount++;

            ShowLevelSuccessUI();
            SaveLevelOutcome();
            return;
        }

        // Level failed
        ShowLevelFailedUI();
    }

    private void ShowLevelSuccessUI()
    {
        OutcomeText.text = "Level complete";
        ShowStars();
    }

    private void ShowLevelFailedUI()
    {
        OutcomeText.text = "Level failed";

        // Hide "Next" Button
        NextBtn.gameObject.SetActive(false);

        // Reset Button Position = right side
        ResetBtn.transform.position = _posNextLevelBtn;
    }

    private void ShowStars()
    {
        Stars.gameObject.SetActive(true);

        // One Star
        if (starsEarnedAmount >= 1)
            Star1.SetActive(true);

        // Two Stars
        if (starsEarnedAmount >= 2)
            Star2.SetActive(true);

        // Three Stars
        if (starsEarnedAmount >= 3)
            Star3.SetActive(true);
    }

    private void SaveLevelOutcome()
    {
        LevelSaveData levelSaveData = new LevelSaveData();
        levelSaveData.levelNumber = SceneManager.GetActiveScene().buildIndex;
        levelSaveData.starsEarned = starsEarnedAmount;
        levelSaveData.stepsTaken = 0;
        levelSaveData.umlElementsUsed = 0;

        SaveManager.Instance.SaveLevel(levelSaveData);
    }
}
