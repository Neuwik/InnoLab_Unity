using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPrefab : MonoBehaviour
{
    public int index;

    public GameObject IndexGO;
    public GameObject Lock;

    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;

    public void SetUnlocked()
    {
        IndexGO.SetActive(true);
        Lock.SetActive(false);
    }

    public void SetSaveData(LevelSaveData saveData)
    {
        SetUnlocked();
        if (saveData.starsEarned >= 1)
            Star1.SetActive(true);
        if (saveData.starsEarned >= 2)
            Star2.SetActive(true);
        if (saveData.starsEarned >= 3)
            Star3.SetActive(true);
    }
}
