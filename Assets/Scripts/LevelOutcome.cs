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

public class LevelOutcome : MonoBehaviour, IResetable
{
    #region DEBUG

    public bool condition1_IsTrue = true;
    public bool condition2_IsTrue = true;
    public bool condition3_IsTrue = true;

    #endregion

    public bool playerDied = false;
    public bool garbageCollected = false;
    private float successQualityPercent = 0;
    public float percentHealth = 0;
    public float percentEnergy = 0;

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

    public Button MenuBtn;
    public Button ResetBtn;
    public Button NextBtn;

    private int starsEarnedAmount = 0;

    //private bool _isLevelActive = true;
    private Vector2 _posNextLevelBtn;
    private Vector2 _posResetLevelBtn;


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
        _posResetLevelBtn = ResetBtn.transform.position;

        MenuBtn.onClick.AddListener(OnClickMenu);
        ResetBtn.onClick.AddListener(OnClickReset);
        NextBtn.onClick.AddListener(OnClickNext);
    }

    // Update is called once per frame
    /*
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
            if(gameManager.steps > forStar_maxSteps)
                condition2_IsTrue = false;

            if(gameManager.UMLElements.Count > forStar_maxUMLElements)
                condition3_IsTrue = false;
        }
    }
    */

    public void ShowLevelOutcome()
    {
        if (!playerDied)
        {
            if (garbageCollected)
            {
                EndLevel(true);
                return;
            }
        }
        EndLevel(false);
    }

    public void Reset()
    {
        playerDied = false;
        garbageCollected = false;
        successQualityPercent = 0;
        percentHealth = 0;
        percentEnergy = 0;
        starsEarnedAmount = 0;
    }

    private void EndLevel(bool isSuccess)
    {
        //_isLevelActive = false;
        LevelOutcomePrefab.SetActive(true);

        // Level success
        if (isSuccess)
        {
            /*
            //DEBUG
            if (condition1_IsTrue)
                starsEarnedAmount++;
            if (condition2_IsTrue)
                starsEarnedAmount++;
            if (condition3_IsTrue)
                starsEarnedAmount++;
            */

            successQualityPercent = (percentHealth + percentEnergy) / 2;

            if (successQualityPercent >= 0.25)
                starsEarnedAmount++;
            if (successQualityPercent >= 0.5)
                starsEarnedAmount++;
            if (successQualityPercent >= 0.75)
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

        // Show "Next" Button
        NextBtn.gameObject.SetActive(true);

        // Reset Button Position = middle
        ResetBtn.transform.position = _posResetLevelBtn;

        ShowStars();
    }

    private void ShowLevelFailedUI()
    {
        OutcomeText.text = "Level failed";

        // Hide "Next" Button
        NextBtn.gameObject.SetActive(false);

        // Reset Button Position = right side
        ResetBtn.transform.position = _posNextLevelBtn;

        //Hide Stars
        Stars.gameObject.SetActive(false);
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
        levelSaveData.successQualityPercent = successQualityPercent;
        levelSaveData.percentHealth = percentHealth;
        levelSaveData.percentEnergy = percentEnergy;

        SaveManager.Instance.SaveLevel(levelSaveData);
    }

    private void OnClickMenu()
    {
        gameManager.LevelManager.LoadLevelSelectionScene();
    }
    private void OnClickReset()
    {
        gameManager.ResetLevel();
        gameObject.SetActive(false);
    }
    private void OnClickNext()
    {
        gameManager.LevelManager.LoadNextLevel();
    }
}
