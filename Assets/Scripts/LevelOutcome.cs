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

    public GameObject LevelOutcomeGO;
    public GameObject Stars;
    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;

    public TMP_Text LevelIndexText;
    public TMP_Text OutcomeText;

    public Button NextBtn;
    public Button ResetBtn;

    private bool _isLevelActive = true;
    private Vector2 _posNextLevelBtn;


    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
            Debug.LogError("GameManager is empty");

        LevelOutcomeGO.SetActive(false);

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
                    ShowLevelOutcome(true);
            }
            // Level failed
            else
            {
                ShowLevelOutcome(false);
            }


            if(gameManager.steps > forStar_maxSteps)
                condition2_IsTrue = false;

            if(gameManager.UMLElements.Count > forStar_maxUMLElements)
                condition3_IsTrue = false;
        }
    }

    private void ShowLevelOutcome(bool isSuccess)
    {
        _isLevelActive = false;
        LevelOutcomeGO.SetActive(true);

        // Level success
        if (isSuccess)
        {
            ShowLevelSuccessUI();
            return;
        }

        // Level failed
        ShowLevelFailedUI();
    }

    private void ShowLevelSuccessUI()
    {
        OutcomeText.text = "Level complete";

        int starsAmount = 0;

        if (condition1_IsTrue)
            starsAmount++;
        if (condition2_IsTrue)
            starsAmount++;
        if (condition3_IsTrue)
            starsAmount++;

        ShowStars(starsAmount);
    }

    private void ShowLevelFailedUI()
    {
        OutcomeText.text = "Level failed";

        // Hide "Next" Button
        NextBtn.gameObject.SetActive(false);

        // Reset Button Position = right side
        ResetBtn.transform.position = _posNextLevelBtn;
    }

    private void ShowStars(int amount)
    {
        Stars.gameObject.SetActive(true);

        // One Star
        if (amount >= 1)
            Star1.gameObject.SetActive(true);

        // Two Stars
        if (amount >= 2)
            Star2.gameObject.SetActive(true);

        // Three Stars
        if (amount >= 3)
            Star3.gameObject.SetActive(true);
    }
}
