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
    private StarCalculationValues calculationValues;
    private int starsEarnedAmount = -1;

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

    //private bool _isLevelActive = true;
    private Vector2 _posNextLevelBtn;
    private Vector2 _posResetLevelBtn;

    private int levelNumber;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameManager == null)
            gameManager = GameManager.Instance;

        if (LevelOutcomePrefab == null)
        {
            LevelOutcomePrefab = gameObject;
        }

        levelNumber = SceneManager.GetActiveScene().buildIndex;
        LevelIndexText.text = "Level " + levelNumber;

        MenuBtn.onClick.AddListener(OnClickMenu);
        ResetBtn.onClick.AddListener(OnClickReset);
        NextBtn.onClick.AddListener(OnClickNext);
    }

    private void Start()
    {
        _posNextLevelBtn = NextBtn.transform.localPosition;
        _posResetLevelBtn = ResetBtn.transform.localPosition;
    }

    public void ShowLevelOutcome(StarCalculationValues values)
    {
        calculationValues = values;
        starsEarnedAmount = StarCalculator.CalculateStarAmount(calculationValues);
        EndLevel(!(starsEarnedAmount < 0));
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        calculationValues = new StarCalculationValues();
        starsEarnedAmount = -1;
        gameObject.SetActive(false);
    }

    private void EndLevel(bool isSuccess)
    {
        // Level success
        if (isSuccess)
        {
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
        ResetBtn.transform.localPosition = _posResetLevelBtn;

        ShowStars();
    }

    private void ShowLevelFailedUI()
    {
        OutcomeText.text = "Level failed";

        // Hide "Next" Button
        NextBtn.gameObject.SetActive(false);

        // Reset Button Position = right side
        ResetBtn.transform.localPosition = _posNextLevelBtn;
        //ResetBtn.transform.localPosition = NextBtn.transform.localPosition;

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
        levelSaveData.levelNumber = levelNumber;
        levelSaveData.starsEarned = starsEarnedAmount;
        //levelSaveData.stepsTaken = 0;
        //levelSaveData.umlElementsUsed = 0;
        levelSaveData.successQualityPercent = calculationValues.SuccessQualityPercent;
        levelSaveData.percentHealth = calculationValues.percentHealth;
        levelSaveData.percentEnergy = calculationValues.percentEnergy;

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
