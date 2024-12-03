using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGrid : MonoBehaviour
{
    public GameObject LeftArrow;
    public GameObject RightArrow;
    public int gridMaxAmount;

    private List<GameObject> _Levels;
    private int _gridPagesCount;
    private int _currentPage;

    // Start is called before the first frame update
    void Start()
    {
        // Get all Levels
        _Levels = new List<GameObject>();
        foreach (Transform child in this.transform)
        {
            _Levels.Add(child.gameObject);
        }

        // Set Sites
        _gridPagesCount = Mathf.CeilToInt((float)_Levels.Count / gridMaxAmount) - 1;
        _currentPage = 0;

        // Deactivate Levels above the first Page
        for (int i = gridMaxAmount; i < _Levels.Count; i++)
        {
            _Levels[i].SetActive(false);
        }

        // Set Arrows
        LeftArrow.GetComponent<Button>().onClick.AddListener(PressLeftArrow);
        RightArrow.GetComponent<Button>().onClick.AddListener(PressRightArrow);

        UpdateArrowVisibility();
    }

    private void PressLeftArrow()
    {
        if (_currentPage <= 0)
            return;

        // Deactivate old Page
        int toDeactivate = _currentPage * gridMaxAmount;
        int stopHere = toDeactivate + gridMaxAmount;

        while (toDeactivate < stopHere && toDeactivate < _Levels.Count)
        {
            _Levels[toDeactivate].SetActive(false);
            toDeactivate++;
        }

        // Activate new Page
        _currentPage--;
        int toActivate = _currentPage * gridMaxAmount;
        stopHere = toActivate + gridMaxAmount;

        while (toActivate < stopHere && toActivate < _Levels.Count)
        {
            _Levels[toActivate].SetActive(true);
            toActivate++;
        }

        UpdateArrowVisibility();
    }

    private void PressRightArrow()
    {
        if (_currentPage >= _gridPagesCount)
            return;

        // Deactivate old Page
        int toDeactivate = _currentPage * gridMaxAmount;
        int stopHere = toDeactivate + gridMaxAmount;

        while (toDeactivate < stopHere && toDeactivate < _Levels.Count)
        {
            _Levels[toDeactivate].SetActive(false);
            toDeactivate++;
        }

        // Activate new Page
        _currentPage++;
        int toActivate = _currentPage * gridMaxAmount;
        stopHere = toActivate + gridMaxAmount;

        while (toActivate < stopHere && toActivate < _Levels.Count)
        {
            _Levels[toActivate].SetActive(true);
            toActivate++;
        }

        UpdateArrowVisibility();
    }

    private void UpdateArrowVisibility()
    {
        LeftArrow.SetActive(_currentPage > 0);
        RightArrow.SetActive(_currentPage < _gridPagesCount);
    }
}
