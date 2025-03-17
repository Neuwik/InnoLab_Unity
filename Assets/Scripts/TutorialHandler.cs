using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _tutorialBoxes = new List<GameObject>();

    private int _currentTutorialIndex;

    void Awake()
    {
        foreach (GameObject tbox in _tutorialBoxes)
        {
            tbox.SetActive(false);
        }
    }
    private void Start()
    {
        StartTutorial();
    }

    public void DisplayNextTutorial()
    {
        if (_currentTutorialIndex >= _tutorialBoxes.Count - 1)
        {
            StopTutorial();
            return;
        }

        _tutorialBoxes[_currentTutorialIndex].SetActive(false);
        ++_currentTutorialIndex;
        _tutorialBoxes[_currentTutorialIndex].SetActive(true);
    }

    public void DisplayPrevTutorial()
    {
        _tutorialBoxes[_currentTutorialIndex].SetActive(false);
        --_currentTutorialIndex;
        if (_currentTutorialIndex < 0)
        {
            _currentTutorialIndex = _tutorialBoxes.Count - 1;

        }
        _tutorialBoxes[_currentTutorialIndex].SetActive(true);
    }

    public void StartTutorial()
    {
        if (_tutorialBoxes.Count <= 0)
        {
            return;
        }
        // Pause the game ?
        // Time.timeScale = 0f;

        _currentTutorialIndex = 0;
        _tutorialBoxes[_currentTutorialIndex].SetActive(true);
    }

    public void StopTutorial()
    {
        if (_tutorialBoxes.Count <= 0)
        {
            return;
        }

        _tutorialBoxes[_currentTutorialIndex].SetActive(false);

        // Resume the game ?
        // Time.timeScale = 1f;
    }
}
