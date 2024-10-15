using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class LevelProgress : MonoBehaviour, IResetable
{
    [SerializeField]
    private GameObject star1;
    [SerializeField]
    private GameObject star2;
    [SerializeField]
    private GameObject star3;

    [SerializeField]
    private TMP_Text TextTrashCount;

    private void Start()
    {
        GameManager.Instance.UpdateLevelProgress();
    }

    public void Reset()
    {
        star1.SetActive(true);
        star2.SetActive(true);
        star3.SetActive(true);
        TextTrashCount.text = "0 / 0";
    }

    private GameObject GetStarByIndex(int index)
    {
        if (index >= 3)
        {
            return star3;
        }
        else if (index >= 2)
        {
            return star2;
        }
        else if (index >= 1)
        {
            return star1;
        }
        else
        {
            return null;
        }
    }

    public void UpdateLevelProgress(StarCalculationValues values)
    {
        TextTrashCount.text = values.collectedTrash + " / " + values.maxTrashCount;

        int stars = StarCalculator.CalculateStarAmount(values, true);

        if (stars < 0)
        {
            stars = 0;
        }

        for (int i = stars + 1; i <= 3; i++)
        {
            GetStarByIndex(i).SetActive(false);
        }
    }
}
